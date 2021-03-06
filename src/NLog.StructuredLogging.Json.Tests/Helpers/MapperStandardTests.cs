﻿using System;
using System.Collections.Generic;
using System.Linq;
using NLog.StructuredLogging.Json.Helpers;
using NUnit.Framework;

namespace NLog.StructuredLogging.Json.Tests.Helpers
{
    [TestFixture]
    public class MapperStandardTests
    {
        private Dictionary<string, object> _result;

        [SetUp]
        public void Setup()
        {
            var logEventInfo = MakeStandardLogEventInfo();
            _result = Mapper.ToDictionary(logEventInfo);
        }

        [Test]
        public void WhenConverted_TheRightNumberOfItemsAreReturned()
        {
            Assert.AreEqual(12, _result.Count);
        }

        [Test]
        public void WhenConverted_TheCorrectKeysAreReturned()
        {
            Assert.True(_result.Keys.Contains("Level"));
            Assert.True(_result.Keys.Contains("LoggerName"));
            Assert.True(_result.Keys.Contains("Message"));
            Assert.True(_result.Keys.Contains("Parameters"));
            Assert.True(_result.Keys.Contains("TimeStamp"));
        }

        [Test]
        public void WhenConverted_TheCorrectExceptionKeysAreReturned()
        {
            Assert.True(_result.Keys.Contains("Exception"));
            Assert.True(_result.Keys.Contains("ExceptionType"));
            Assert.True(_result.Keys.Contains("ExceptionMessage"));
            Assert.True(_result.Keys.Contains("ExceptionStackTrace"));
            Assert.True(_result.Keys.Contains("ExceptionFingerprint"));
        }

        [Test]
        public void WhenConverted_ThePropertyKeyIsConvertedToProperties()
        {
            Assert.False(_result.Keys.Contains("Properties"));
            Assert.True(_result.Keys.Contains("PropertyOne"));
            Assert.True(_result.Keys.Contains("PropertyTwo"));

            Assert.AreEqual("This value is in property one", _result["PropertyOne"]);
            Assert.AreEqual("2", _result["PropertyTwo"]);
        }

        [Test]
        public void WhenConverted_TheExceptionIsExpanded()
        {
            Assert.AreEqual("System.Exception: Outer Exception ---> System.Exception: Inner Exception\r\n   --- End of inner exception stack trace ---", _result["Exception"]);
        }

        private LogEventInfo MakeStandardLogEventInfo()
        {
            return new LogEventInfo
            {
                Exception = new Exception("Outer Exception", new Exception("Inner Exception")),
                Level = LogLevel.Error,
                LoggerName = "ExampleLoggerName",
                Message = "Example Message",
                Parameters = new object[] { "One", 1234 },
                Properties = { { "PropertyOne", "This value is in property one" }, { "PropertyTwo", 2 } },
                TimeStamp = new DateTime(2014, 1, 2, 3, 4, 5, 6),
            };
        }
    }
}
