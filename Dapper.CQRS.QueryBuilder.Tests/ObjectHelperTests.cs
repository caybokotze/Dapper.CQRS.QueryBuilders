using System.Numerics;
using Dapper.CQRS.QueryBuilders;
using NExpect;
using NUnit.Framework;
using static NExpect.Expectations;

namespace Dapper.CQRS.QueryBuilder.Tests
{
    [TestFixture]
    public class ObjectHelperTests
    {
        public class GetObjectPropertiesTests
        {
            [Test]
            public void SingleVariableShouldReturnObjectProperties()
            {
                // arrange
                var name = "John";
                // act
                var result = name.GetObjectProperties();
                // assert
                Expect(result.Count).To.Equal(2);
            }
            
            [Test]
            public void SimpleObjectShouldReturnObjectProperties()
            {
                // arrange
                var person = new {
                    Name = "John",
                    Surname = "Mkinney",
                    Age = 84
                };
                // act
                var result = person.GetObjectProperties();
                // assert
                Expect(result.Count).To.Equal(3);
            }

            [Test]
            public void SimpleObjectShouldReturnObjectPropertiesWithCorrectNames()
            {
                // arrange
                var person = new
                {
                    Name = "John",
                    surname = "Mkinney",
                    Age = 65
                };
                // act
                var result = person.GetObjectProperties();
                // assert
                Expect(result[0].Name).To.Equal("Name");
                Expect(result[1].Name).To.Equal("surname");
                Expect(result[2].Name).To.Equal("Age");
            }

            [Test]
            public void SimpleClassInstanceShouldReturnObjectPropertiesWithCorrectNames()
            {
                // arrange
                var person = new Person
                {
                    Name = "John",
                    Surname = "Mkinney",
                    Age = 65
                };
                // act
                var result = person.GetObjectProperties();
                // assert
                Expect(result[0].Name).To.Equal("Name");
                Expect(result[1].Name).To.Equal("Surname");
                Expect(result[2].Name).To.Equal("Age");
            }
            
            [Test]
            public void ComplexClassInstanceShouldReturnObjectPropertiesWithCorrectNames()
            {
                // arrange
                var person = new ComplexAnimal
                {
                    Person = new()
                    {
                        Name = "John",
                        Surname = "Sulley",
                        Age = 21
                    },
                    Variant = 3
                };
                // act
                var result = person.GetObjectProperties();
                // assert
                Expect(result[0].Name).To.Equal("Person");
                Expect(result[1].Name).To.Equal("Variant");
            }
        }

        [TestFixture]
        public class ReturnClassAsDataTableTests
        {
            [Test]
            public void ClassInstanceShouldReturnDictionaryOfProperties()
            {
                // arrange
                var person = new Person()
                {
                    Name = "John",
                    Surname = "Mulaney",
                    Age = 28
                };
                // act
                var result = person.ReturnClassAsDataTable<Person>();
                // assert
                Expect(result.Columns.Count).To.Equal(3);
            }
            
            [Test]
            public void TypeGenericShouldBeClassAndDoesNotEffectOutput()
            {
                // arrange
                var person = new Person
                {
                    Name = "John",
                    Surname = "Mulaney",
                    Age = 28
                };
                // act
                var result = person.ReturnClassAsDataTable<SomeRandomClass>();
                // assert
                Expect(result.Columns.Count).To.Equal(3);
            }
            
            [Test]
            public void ComplexClassInstanceShouldReturnDictionaryOfProperties()
            {
                // arrange
                var person = new ComplexAnimal()
                {
                    Person = new Person
                    {
                        Name = "John",
                        Surname = "Mulaney",
                        Age = 28
                    },
                    Variant = 3
                };
                // act
                var result = person.ReturnClassAsDataTable<ComplexAnimal>();
                // assert
                Expect(result.Columns.Count).To.Equal(2);
            }
        }

        [TestFixture]
        public class RemovePropertiesFromDatatableArgumentTests
        {
            [Test]
            public void ShouldRemoveAllPropertiesFromDatatable()
            {
                // arrange
                var person = new Person()
                {
                    Name = "John",
                    Surname = "Williams",
                    Age = 20
                };
                var dictionary = person.ReturnClassAsDataTable<Person>();
                var properties = person.GetObjectProperties();
                // act
                dictionary.RemovePropertiesFromDatatable(properties);
                // assert
                Expect(dictionary.Columns.Count).To.Equal(0);
            }
            
            [Test]
            public void ShouldRemoveSpecificPropertiesFromDatatable()
            {
                // arrange
                var person = new Person()
                {
                    Name = "John",
                    Surname = "Williams",
                    Age = 20
                };
                var removeProps = new
                {
                    Name = ""
                };
                var dictionary = person.ReturnClassAsDataTable<Person>();
                var properties = removeProps.GetObjectProperties();
                // act
                dictionary.RemovePropertiesFromDatatable(properties);
                // assert
                Expect(dictionary.Columns.Count).To.Equal(2);
                Expect(dictionary.Columns.Contains(nameof(person.Age))).To.Be.True();
                Expect(dictionary.Columns.Contains(nameof(person.Surname))).To.Be.True();
                Expect(dictionary.Columns.Contains(nameof(person.Name))).Not.To.Be.True();
            }
        }
        
        class Person
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Age { get; set; }
        }

        class SomeRandomClass
        {
            
        }

        class ComplexAnimal
        {
            public Person Person { get; set; }
            public int Variant { get; set; }
        }
    }
}