// using System;
// using MongoDB.Bson.Serialization.Attributes;
//
// namespace Navtrack.DataAccess.Model.Devices.Messages;
//
// public class IOElement
// {
//     [BsonElement("di")]
//     public bool?[]? DigitalInputs { get; set; }
//     
//     [BsonElement("do")]
//     public bool?[]? DigitalOutputs { get; set; }
//     
//     [BsonElement("ai")]
//     public double?[]? AnalogInputs { get; set; }
//     
//     [BsonElement("ao")]
//     public double?[]? AnalogOutputs { get; set; }
//
//     private static T[] SetValue<T>(T[]? array, int index, T value)
//     {
//         if (array == null)
//         {
//             array = new T[index];
//         }
//         else if (index > array.Length)
//         {
//             Array.Resize(ref array, index);
//         }
//
//         array[index-1] = value;
//         
//         return array;
//     }
//
//     public void SetDigitalInput(int index, bool value)
//     {
//         DigitalInputs = SetValue(DigitalInputs, index, value);
//     }
//
//     public void SetDigitalOutput(int index, bool value)
//     {
//         DigitalOutputs = SetValue(DigitalOutputs, index, value);
//     }
//
//     public void SetAnalogInput(int index, double value)
//     {
//         AnalogInputs = SetValue(AnalogInputs, index, value);
//     }
//
//     public void SetAnalogOutput(int index, double value)
//     {
//         AnalogOutputs = SetValue(AnalogOutputs, index, value);
//     }
// }