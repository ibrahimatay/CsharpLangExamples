﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace DynamicTypes
{
    /// <summary>
    /// https://github.com/IronLanguages/dlr
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/types/using-type-dynamic
    /// https://channel9.msdn.com/blogs/matthijs/c-40-and-beyond-by-anders-hejlsberg
    /// https://channel9.msdn.com/shows/Going+Deep/Inside-C-40-dynamic-type-optional-parameters-more-COM-friendly/
    /// </summary>
    class Program
    {
        public dynamic Property { get; set; }

        static void Main(string[] args)
        {
            Person person01 = new Person("İbrahim", "ATAY"); // clr
            Console.WriteLine($"CLR Type of Person - Type:{person01.GetType()}");

            // Person person02 = new ExpandoObject(); // give the compilation error because Person is clr object

            dynamic person03 = new ExpandoObject(); // dlr object
            person03.FirstName = "İbrahim";
            person03.LastName = "ATAY";
            Console.WriteLine($"Dynamic Type of Person - Type:{person03.GetType()}");

            var person04 = new Person("İbrahim", "ATAY");

            // give the compilation error because var variable is not dynamic type.
            // var declarations are resolved at compile-time.
            // https://stackoverflow.com/questions/3442821/dynamic-vs-object-type
            // https://stackoverflow.com/questions/961581/whats-the-difference-between-dynamic-c-4-and-var
            //var person05 = new ExpandoObject();
            //person05.FirstName = "İbrahim";
            //person05.LastName = "ATAY";

            dynamic person06 = new ExpandoObject();
            person06.FirstName = "İbrahim";
            person06.LastName = "ATAY";

            var person07 = person06; // but do not access property
            //person07.FirstName //compilation error

            dynamic factory = Activator.CreateInstance(typeof(Factory));
            Console.WriteLine($"Factory is {factory.GetFactoryName()}");


            // using Dynamic type with extension methods
            dynamic today = DateTime.Today;
            // give the runtime exception
            //Console.WriteLine(today.ToUKFormat());
            //Microsoft.CSharp.RuntimeBinder.RuntimeBinderException: ''System.DateTime' does not contain a definition for 'ToUKFormat''
            
            Console.WriteLine(DateTimeExtensions.ToUKFormat(today));

            // List & Dictionary
            List<dynamic> person08 = new List<dynamic>()
            {
                new Person("İbrahim", "ATAY"),
                new Person("Gizem", "ATAY")
            };

            Dictionary<string, dynamic> dictionary = new Dictionary<string, dynamic>()
            {
                {"IA", new Person("İbrahim", "ATAY")},
                {"GA", new Person("Gizem", "ATAY")}
            };

            if (IsDictionary(dictionary))
            {
                Console.WriteLine("dictionary is dynamic dictionary");
            }
        }

        public static void Sum(int val01) { }
        public static void Sum(dynamic val01) { }
        public static void Sum(int val01, dynamic val02) { }
        public static void Sum(dynamic val01, dynamic val02) { }

        public dynamic Foo()=> new ExpandoObject();
        //public var Foo() => new ExpandoObject();  // compilation error

        public static dynamic Bar() => 1;

        public object Baz() => new ExpandoObject(); // return by dlr object

        public static bool IsDictionary(object o)
        {
            if (o == null) return false;
            return o is IDictionary &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }
    }

    class Person
    {
        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; set; }
        public string LastName{get; set; }
    }

    class Factory
    {
        public string GetFactoryName() => "Big Factory!";
    }

    static class DateTimeExtensions
    {
        public static string ToUKFormat(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }
    }
}
