const assert = require("node:assert/strict");
const { readFileSync, existsSync } = require("node:fs");
const path = require("node:path");
const test = require("node:test");
const { transpileModule, ModuleKind, ScriptTarget } = require("typescript");
const { createStore } = require("jotai/vanilla");
const { QueryClient, MutationObserver } = require("@tanstack/react-query");

const storageKey = "Navtrack:Authentication:OpenIddict";
const hookPath = path.resolve(
  __dirname,
  "../src/hooks/app/authentication/useAuthentication.ts"
);

function deferred() {
  let resolve;
  let reject;
  const promise = new Promise((yes, no) => {
    resolve = yes;
    reject = no;
  });
  return { promise, resolve, reject };
}

function token(accessToken = "old-access", expired = true) {
  return {
    accessToken,
    refreshToken: `${accessToken}-refresh`,
    expiryDate: new Date(
      Date.now() + (expired ? -60000 : 3600000)
    ).toISOString(),
    date: new Date().toISOString()
  };
}

function response(accessToken = "new-access") {
  return {
    access_token: accessToken,
    refresh_token: `${accessToken}-refresh`,
    expires_in: 3600,
    token_type: "Bearer",
    scope: "openid offline_access"
  };
}

function oauthError(error, status = 400, extra = {}) {
  return {
    isAxiosError: true,
    response: { status, data: { error, ...extra } }
  };
}

function environment(initial = {}) {
  let persisted = JSON.stringify(initial);
  const locks = new Map();
  return {
    storage: {
      getItem: async () => persisted,
      setItem: async (_, value) => {
        persisted = value;
      },
      removeItem: async () => {
        persisted = null;
      }
    },
    locks: {
      request(name, action) {
        const result = (locks.get(name) ?? Promise.resolve()).then(action);
        locks.set(
          name,
          result.catch(() => {})
        );
        return result;
      }
    },
    read: () => JSON.parse(persisted ?? "{}")
  };
}

function runtime(env = environment(), { web = true } = {}) {
  const store = createStore();
  const queryClient = new QueryClient({
    defaultOptions: { mutations: { retry: false, gcTime: 0 } }
  });
  const modules = new Map();
  const resolvedValues = new WeakMap();
  const requests = [];
  const pending = [];

  // Replace only React's hook bindings; atoms, persistence helpers, mutations,
  // and the actual authentication/token/logout code execute normally.
  const mocks = {
    react: { useCallback: (callback) => callback },
    jotai: {
      ...require("jotai/vanilla"),
      useAtomValue(atom) {
        const value = store.get(atom);
        if (value instanceof Promise) {
          if (!resolvedValues.has(value)) throw value;
          return resolvedValues.get(value);
        }
        return value;
      }
    },
    "@tanstack/react-query": {
      useQueryClient: () => queryClient,
      useMutation(options) {
        const observer = new MutationObserver(queryClient, options);
        return { mutateAsync: (variables) => observer.mutate(variables) };
      }
    },
    "@react-native-async-storage/async-storage": env.storage
  };
  const localMocks = {
    "state/store.ts": { jotaiStore: store },
    "utils/web.ts": { IS_WEB: web },
    "utils/asyncLocalStorage.ts": { AsyncLocalStorage: env.storage },
    "axios/axiosInstance.ts": {
      axiosInstance(config) {
        const request = deferred();
        requests.push({
          ...config,
          data:
            typeof config.data === "string"
              ? Object.fromEntries(new URLSearchParams(config.data))
              : config.data
        });
        pending.push(request);
        return request.promise;
      }
    }
  };

  function load(filename) {
    if (modules.has(filename)) return modules.get(filename).exports;
    const module = { exports: {} };
    modules.set(filename, module);
    const { outputText } = transpileModule(readFileSync(filename, "utf8"), {
      compilerOptions: {
        module: ModuleKind.CommonJS,
        target: ScriptTarget.ES2022,
        esModuleInterop: true
      }
    });
    const localRequire = (name) => {
      if (mocks[name]) return mocks[name];
      if (!name.startsWith(".")) return require(name);
      let resolved = path.resolve(path.dirname(filename), name);
      if (!existsSync(resolved)) resolved += ".ts";
      const key = Object.keys(localMocks).find((suffix) =>
        resolved.endsWith(`/src/${suffix}`)
      );
      return key ? localMocks[key] : load(resolved);
    };
    new Function("require", "module", "exports", "navigator", outputText)(
      localRequire,
      module,
      module.exports,
      web ? { locks: env.locks } : undefined
    );
    return module.exports;
  }

  const { useAuthentication } = load(hookPath);
  return {
    requests,
    pending,
    queryClient,
    read: env.read,
    async hook(props) {
      while (true) {
        try {
          return useAuthentication(props);
        } catch (value) {
          if (!(value instanceof Promise)) throw value;
          resolvedValues.set(value, await value);
        }
      }
    }
  };
}

async function waitFor(predicate) {
  for (let attempt = 0; attempt < 100; attempt++) {
    if (predicate()) return;
    await new Promise((resolve) => setImmediate(resolve));
  }
  assert.fail("Expected authentication operation did not arrive");
}

for (const web of [true, false]) {
  test(`clearing errors/link state preserves tokens (${web ? "web" : "mobile"})`, async () => {
    const original = token();
    const app = runtime(
      environment({
        token: original,
        error: "failure",
        external: { provider: "google" }
      }),
      { web }
    );
    const auth = await app.hook();
    await auth.clearErrors();
    assert.deepEqual(app.read().token, original);
    assert.equal(app.read().error, undefined);
    assert.equal(app.read().external.provider, "google");
    await auth.clearExternal();
    assert.deepEqual(app.read().token, original);
    assert.equal(app.read().external, undefined);
  });
}

test("consumers share login status; callback waits for a saved successful session", async () => {
  const app = runtime();
  let callbacks = 0;
  const auth = await app.hook({
    onLogin: () => {
      assert.equal(app.read().token.accessToken, "signed-in");
      callbacks++;
    }
  });
  const login = auth.internalLogin({
    email: "user@example.com",
    password: "password"
  });
  assert.equal((await app.hook()).isSigningIn, true);
  assert.equal((await app.hook()).isLoading, true);
  assert.equal(callbacks, 0);
  await waitFor(() => app.pending.length === 1);
  app.pending[0].resolve(response("signed-in"));
  assert.equal(await login, true);
  assert.equal(callbacks, 1);
  assert.equal((await app.hook()).isSigningIn, false);
});

test("failed social login resolves false and preserves the account-link flow", async () => {
  const app = runtime();
  let callbacks = 0;
  const auth = await app.hook({ onLogin: () => callbacks++ });
  const login = auth.externalLogin(
    "provider-code",
    "google",
    "authorization_code"
  );
  await waitFor(() => app.pending.length === 1);
  assert.equal(app.requests[0].data.credential_type, "authorization_code");
  app.pending[0].reject(
    oauthError("invalid_grant", 400, {
      code: "LOGIN_000002",
      token: "linking-token"
    })
  );
  assert.equal(await login, false);
  assert.equal(callbacks, 0);
  assert.deepEqual(app.read().external, {
    provider: "google",
    token: "linking-token",
    credentialType: "id_token"
  });
  assert.equal((await app.hook()).isLoading, false);

  const external = app.read().external;
  const linked = auth.externalLogin(
    external.token,
    external.provider,
    external.credentialType,
    "linking-password"
  );
  await waitFor(() => app.pending.length === 2);
  assert.equal(app.requests[1].data.code, "linking-token");
  assert.equal(app.requests[1].data.credential_type, "id_token");
  assert.equal(app.requests[1].data.password, "linking-password");
  app.pending[1].resolve(response());
  assert.equal(await linked, true);
  assert.equal(callbacks, 1);
});

test("mobile Google login sends its ID token with an explicit credential type", async () => {
  const app = runtime(environment(), { web: false });
  const auth = await app.hook();
  const login = auth.externalLogin("google-id-token", "google", "id_token");
  await waitFor(() => app.pending.length === 1);
  assert.equal(app.requests[0].data.code, "google-id-token");
  assert.equal(app.requests[0].data.credential_type, "id_token");
  app.pending[0].resolve(response());
  assert.equal(await login, true);
});

test("concurrent consumers share one refresh without showing login loading", async () => {
  const app = runtime(environment({ token: token() }));
  const first = (await app.hook()).getAccessToken();
  const second = (await app.hook()).getAccessToken();
  await waitFor(() => app.pending.length === 1);
  const auth = await app.hook();
  assert.equal(auth.isRefreshing, true);
  assert.equal(auth.isSigningIn, false);
  assert.equal(auth.isLoading, false);
  app.pending[0].resolve(response());
  assert.deepEqual(await Promise.all([first, second]), [
    "new-access",
    "new-access"
  ]);
  assert.equal(app.requests.length, 1);
  assert.equal((await app.hook()).isRefreshing, false);
});

for (const [name, failure] of [
  ["offline", new Error("offline")],
  ["server error", oauthError("server_error", 503)],
  ["rate limit", oauthError("slow_down", 429)]
]) {
  test(`${name} during refresh preserves credentials and cache, and allows retry`, async () => {
    const original = token();
    const app = runtime(environment({ token: original }));
    app.queryClient.setQueryData(["user"], { id: 1 });
    const auth = await app.hook();
    const first = auth.getAccessToken();
    const failed = assert.rejects(first, (error) => error === failure);
    await waitFor(() => app.pending.length === 1);
    app.pending[0].reject(failure);
    await failed;
    assert.deepEqual(app.read().token, original);
    assert.deepEqual(app.queryClient.getQueryData(["user"]), { id: 1 });
    assert.equal((await app.hook()).isRefreshing, false);
    const retry = auth.getAccessToken();
    await waitFor(() => app.pending.length === 2);
    app.pending[1].resolve(response());
    assert.equal(await retry, "new-access");
  });
}

test("rejected refresh credential ends the session and clears user cache", async () => {
  const app = runtime(environment({ token: token() }));
  app.queryClient.setQueryData(["user"], { id: 1 });
  const refresh = (await app.hook()).getAccessToken();
  const failed = assert.rejects(refresh);
  await waitFor(() => app.pending.length === 1);
  app.pending[0].reject(oauthError("invalid_grant"));
  await failed;
  assert.equal(app.read().token, undefined);
  assert.equal(app.queryClient.getQueryData(["user"]), undefined);
});

test("a login response arriving after logout cannot restore the session or fire its callback", async () => {
  const app = runtime();
  let callbacks = 0;
  const auth = await app.hook({ onLogin: () => callbacks++ });
  const login = auth.internalLogin({ email: "old", password: "password" });
  await waitFor(() => app.pending.length === 1);
  await auth.logout();
  app.pending[0].resolve(response("stale"));
  assert.equal(await login, false);
  assert.equal(app.read().token, undefined);
  assert.equal(callbacks, 0);
});

test("a newer login wins even when the older response arrives last", async () => {
  const app = runtime();
  const auth = await app.hook();
  const oldLogin = auth.internalLogin({ email: "old", password: "password" });
  await waitFor(() => app.pending.length === 1);
  const newLogin = auth.internalLogin({ email: "new", password: "password" });
  await waitFor(() => app.pending.length === 2);
  app.pending[1].resolve(response("new-user"));
  assert.equal(await newLogin, true);
  app.pending[0].resolve(response("old-user"));
  assert.equal(await oldLogin, false);
  assert.equal(app.read().token.accessToken, "new-user");
});

for (const fails of [false, true]) {
  test(`old refresh ${fails ? "failure" : "success"} cannot overwrite a newer login`, async () => {
    const app = runtime(environment({ token: token() }));
    const auth = await app.hook();
    const refresh = auth.getAccessToken();
    const result = fails ? assert.rejects(refresh) : refresh;
    await waitFor(() => app.pending.length === 1);
    const login = auth.internalLogin({ email: "new", password: "password" });
    await waitFor(() => app.pending.length === 2);
    app.pending[1].resolve(response("new-user"));
    assert.equal(await login, true);
    app.queryClient.setQueryData(["user"], { id: 2 });
    if (fails) app.pending[0].reject(oauthError("invalid_grant"));
    else app.pending[0].resolve(response("old-user"));
    assert.equal(await result, undefined);
    assert.equal(app.read().token.accessToken, "new-user");
    assert.deepEqual(app.queryClient.getQueryData(["user"]), { id: 2 });
  });
}

test("logout pins its credential and cannot clear a login completed while revocation is pending", async () => {
  const app = runtime(environment({ token: token("old-user", false) }));
  const auth = await app.hook();
  const logout = auth.logout();
  await waitFor(() => app.pending.length === 1);
  assert.equal((await app.hook()).isSigningOut, true);
  assert.equal(app.requests[0].url, "/auth/logout");
  assert.equal(app.requests[0].headers.Authorization, "Bearer old-user");
  const login = auth.internalLogin({ email: "new", password: "password" });
  await waitFor(() => app.pending.length === 2);
  app.pending[1].resolve(response("new-user"));
  await login;
  app.queryClient.setQueryData(["user"], { id: 2 });
  app.pending[0].resolve();
  await logout;
  assert.equal(app.read().token.accessToken, "new-user");
  assert.deepEqual(app.queryClient.getQueryData(["user"]), { id: 2 });
});

test("separate browser tabs serialize refresh and reuse the rotated token", async () => {
  const env = environment({ token: token() });
  const first = runtime(env);
  const second = runtime(env);
  const refreshA = (await first.hook()).getAccessToken();
  const refreshB = (await second.hook()).getAccessToken();
  await waitFor(() => first.pending.length === 1);
  assert.equal(second.pending.length, 0);
  first.pending[0].resolve(response());
  assert.deepEqual(await Promise.all([refreshA, refreshB]), [
    "new-access",
    "new-access"
  ]);
  assert.equal(second.requests.length, 0);
});

test("logout in another tab invalidates a pending login response", async () => {
  const env = environment();
  const first = runtime(env);
  const second = runtime(env);
  const login = (await first.hook()).internalLogin({
    email: "old",
    password: "password"
  });
  await waitFor(() => first.pending.length === 1);
  await (await second.hook()).logout();
  first.pending[0].resolve(response("stale"));
  assert.equal(await login, false);
  assert.equal(env.read().token, undefined);
});

test("an older login settling does not clear a newer login's loading flag", async () => {
  const app = runtime();
  const auth = await app.hook();
  const oldLogin = auth.internalLogin({ email: "old", password: "password" });
  await waitFor(() => app.pending.length === 1);
  const newLogin = auth.internalLogin({ email: "new", password: "password" });
  await waitFor(() => app.pending.length === 2);
  app.pending[0].resolve(response("old-user"));
  assert.equal(await oldLogin, false);
  assert.equal((await app.hook()).isSigningIn, true);
  app.pending[1].resolve(response("new-user"));
  assert.equal(await newLogin, true);
  assert.equal((await app.hook()).isSigningIn, false);
});

test("mobile refresh completing after logout starts cannot restore its old token", async () => {
  const app = runtime(environment({ token: token() }), { web: false });
  const auth = await app.hook();
  const refresh = auth.getAccessToken();
  await waitFor(() => app.pending.length === 1);
  const logout = auth.logout();
  await waitFor(() => app.pending.length === 2);
  app.pending[1].resolve(response("logout-token"));
  await waitFor(() => app.pending.length === 3);
  app.pending[2].resolve();
  await logout;
  app.pending[0].resolve(response("stale"));
  assert.equal(await refresh, undefined);
  assert.equal(app.read().token, undefined);
});

test("a pending logout cannot revoke a newer login from another tab", async () => {
  const env = environment({ token: token() });
  const first = runtime(env);
  const second = runtime(env);
  const third = runtime(env);
  const refresh = (await first.hook()).getAccessToken();
  await waitFor(() => first.pending.length === 1);
  const logout = (await second.hook()).logout();
  // Observe the logout's session marker before starting the new login.
  await waitFor(() => env.read().sessionId !== undefined);
  const login = (await third.hook()).internalLogin({
    email: "new",
    password: "password"
  });
  await waitFor(() => third.pending.length === 1);
  third.pending[0].resolve(response("new-user"));
  assert.equal(await login, true);
  first.pending[0].resolve(response("stale"));
  assert.equal(await refresh, undefined);
  await logout;
  assert.equal(second.requests.length, 0);
  assert.equal(env.read().token.accessToken, "new-user");
});

test("clearing state waits for asynchronous mobile persistence without losing a concurrent update", async () => {
  const env = environment({
    token: token(),
    error: "failure",
    external: { provider: "google" }
  });
  const write = deferred();
  const setItem = env.storage.setItem;
  let writes = 0;
  env.storage.setItem = async (...args) => {
    if (++writes === 1) await write.promise;
    return setItem(...args);
  };
  const app = runtime(env, { web: false });
  const auth = await app.hook();
  const errors = auth.clearErrors();
  await waitFor(() => writes === 1);
  const external = auth.clearExternal();
  assert.equal(writes, 1);
  write.resolve();
  await Promise.all([errors, external]);
  assert.equal(app.read().token.accessToken, "old-access");
  assert.equal(app.read().external, undefined);
  assert.equal(app.read().error, undefined);
});
