using Xunit;
using DictionaryDeserializer;
using System.Collections.Generic;

namespace DictionaryDeserializer.Tests;
public class DeserializerTester : DictionaryDeserializer
{
    public class TestClass
    {
        public int X {get; set;}
        public int Y {get; set;}
        public float Single { get; set; } 
        public string? String { get; set; } 
    }


    [Fact]
    public void CreateObject()
    {
        Dictionary<string, string> dictionary = new();
        dictionary.Add(nameof(TestClass.X), "2345");
        dictionary.Add(nameof(TestClass.Y), "6789");
        dictionary.Add(nameof(TestClass.Single), "123.5");
        dictionary.Add(nameof(TestClass.String), "some string data");

        TestClass result = new DictionaryDeserializer().DeserializeFrom<TestClass>(dictionary);
        Assert.Equal(result.X, 2345);
        Assert.Equal(result.Y, 6789);
        Assert.Equal(result.Single, 123.5);
        Assert.Equal(result.String, "some string data");

    }
    [Fact]
    public void TestTypesCaster()
    {
        List<object> tests = new(){ 1, 223.31M,  "String", 1e308, 1234.4532F };
        foreach(var element in tests)
        {
            string s =  element.ToString() ?? "";
            var result = ParseStringTo(s, element.GetType()); 
            Assert.Equal(element, result);
        }

        
    }
}
