using System;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using Navtrack.DataAccess.Services.Settings;
using Navtrack.Library.DI;

namespace Navtrack.Common.Services;

[Service(typeof(ISettingProvider))]
public class SettingProvider : ISettingProvider
{
    private readonly ISettingDataService settingDataService;

    public SettingProvider(ISettingDataService settingDataService)
    {
        this.settingDataService = settingDataService;
    }

    public async Task<T> Get<T>()
    {
        string key = typeof(T).Name;

        T value = await settingDataService.GetSetting<T>(key);

        return value;
    }

    public Task Set<T>(T value)
    {
        string key = typeof(T).Name;

        return settingDataService.SaveSetting(key, value);
    }

    public async Task<X> Get<T, X>(Expression<Func<T, X>> expression)
    {
        string key = GetKey(expression);

        string stringValue = await settingDataService.GetSetting(key);

        if (typeof(T) != typeof(string))
        {
            if (string.IsNullOrEmpty(stringValue))
            {
                return default;
            }

            X val = JsonSerializer.Deserialize<X>(stringValue);

            return val;
        }

        return (X)Convert.ChangeType(stringValue, typeof(X));
    }

    public Task Save<T, X>(Expression<Func<T, X>> expression, X value)
    {
        string key = GetKey(expression);

        string val = typeof(X) == typeof(string) ? value.ToString() : JsonSerializer.Serialize(value);

        return settingDataService.SetSetting(key, val);
    }

    private static string GetKey<TObject, TMember>(Expression<Func<TObject, TMember>> property)
    {
        MemberExpression body = property.Body as MemberExpression;
        body ??= ((UnaryExpression)property.Body).Operand as MemberExpression;

        if (body != null)
        {
            return $"{typeof(TObject).Name}.{body.Member.Name}";
        }

        throw new ArgumentException("Could not generate key.");
    }
}