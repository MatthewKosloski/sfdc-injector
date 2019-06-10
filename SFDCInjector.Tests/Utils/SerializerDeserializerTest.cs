using NUnit.Framework;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
using SFDCInjector.Utils;

namespace SFDCInjector.Tests.Utils
{

    /// <summary>
    /// Tests for SFDC Injector's SerializerDeserializer class.
    /// </summary>
    [TestFixture]
    public class SerializerDeserializerTest
    {
        /// <summary>
        /// A simple class used to test serialization methods.
        /// </summary>
        [DataContract]
        class Person
        {
            [DataMember(Name="name")]
            public string Name { get; set; }

            [DataMember(Name="age")]
            public int Age { get; set; }

            public override bool Equals(object obj)
            {
                bool isNull = Object.ReferenceEquals(null, obj),
                    isItself = Object.ReferenceEquals(this, obj),
                    isSameType = obj.GetType() == this.GetType();

                if (isNull) 
                {
                    return false;
                }
                if (isItself || isSameType)
                {
                    return true;
                }
                
                return IsEqual((Person) obj);
            }

            private bool IsEqual(Person person)
            {
                return (Name == person.Name && Age == person.Age);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    // Choose large primes to avoid hashing collisions
                    const int HashingBase = (int) 2166136261;
                    const int HashingMultiplier = 16777619;

                    int hash = HashingBase;
                    hash = (hash * HashingMultiplier) ^ 
                    (!Object.ReferenceEquals(null, Name) ? Name.GetHashCode() : 0);
                    hash = (hash * HashingMultiplier) ^ 
                    (!Object.ReferenceEquals(null, Age) ? Age.GetHashCode() : 0);
                    return hash;
                }
            }
        }

        /// <summary>
        /// Maps a Person object to its corresponding JSON.
        /// </summary>
        private Dictionary<string, Person> _dict;

        [OneTimeSetUp]
        public void Init()
        {
            _dict = new Dictionary<string, Person>();

            _dict.Add("{\"age\":-1,\"name\":\"John Doe\"}", new Person { Name = "John Doe", Age = -1 });
            _dict.Add("{\"age\":0,\"name\":\"Jane Doe\"}", new Person { Name = "Jane Doe", Age = 0});
            _dict.Add("{\"age\":1,\"name\":\"\"}", new Person { Name = "", Age = 1});
            _dict.Add("{\"age\":100,\"name\":\" \"}", new Person { Name = " ", Age = 100});
            _dict.Add("{\"age\":25,\"name\":null}", new Person { Name = null, Age = 25});
        }

        /// <summary>
        /// Tests the SerializeTypeToJson method.
        /// </summary>
        [Test]
        public void SerializeTypeToJsonTest()
        {
            foreach(KeyValuePair<string, Person> kvp in _dict)
            {
                string expected = kvp.Key;
                string actual = SerializerDeserializer.SerializeTypeToJson<Person>(kvp.Value);
                Assert.That(expected, Is.EqualTo(actual));
            }
        }

        /// <summary>
        /// Tests the DeserializeJsonToType method.
        /// </summary>
        [Test, Description("Tests the DeserializeJsonToType method.")]
        public void DeserializeJsonToTypeTest()
        {
            foreach(KeyValuePair<string, Person> kvp in _dict)
            {
                Person expected = kvp.Value;
                Person actual = SerializerDeserializer.DeserializeJsonToType<Person>(kvp.Key);
                Assert.That(expected, Is.EqualTo(actual));
            }
        }
    }
}