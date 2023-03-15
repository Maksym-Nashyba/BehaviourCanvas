using System;
using Code.Runtime.BehaviourGraphSerialization;
using NUnit.Framework;

namespace Code.Tests.BehaviourCanvasEditorTests
{
    public class StateArgumentMapping
    {
        [Test]
        public void Any_MapsToEmpty()
        {
            Assert.DoesNotThrow(()=>ParameterSet.Empty.MapTo(ParameterSet.Empty, null));
            ParameterSet oneParameter = new ParameterSet(new Parameter(typeof(Single), "parameterName"));
            object[] oneArgument = { 1f };
            Assert.DoesNotThrow(()=>ParameterSet.Empty.MapTo(oneParameter, oneArgument));
        }

        [Test]
        public void Empty_OnlyMapsToEmpty()
        {
            Assert.DoesNotThrow(()=>ParameterSet.Empty.MapTo(ParameterSet.Empty, null));
            ParameterSet oneParameter = new ParameterSet(new Parameter(typeof(Single), "parameterName"));
            Assert.Throws<ArgumentException>(()=>ParameterSet.Empty.MapTo(oneParameter, null));
        }
        
        [Test]
        public void Superset_Throws()
        {
            ParameterSet oneParameter = new ParameterSet(
                new Parameter(typeof(Single), "firstParameterName"));
            object[] oneArgument = { 1f };
            
            ParameterSet twoParameters = new ParameterSet(
                new Parameter(typeof(Single), "firstParameterName"),
                new Parameter(typeof(String), "secondsParameterName"));
            
            Assert.Throws<ArgumentException>(()=>oneParameter.MapTo(twoParameters, oneArgument));
        }
        
        [Test]
        public void DirectSubset_Maps()
        {
            ParameterSet oneParameter = new ParameterSet(
                new Parameter(typeof(Single), "firstParameterName"));
            
            ParameterSet twoParameters = new ParameterSet(
                new Parameter(typeof(Single), "firstParameterName"),
                new Parameter(typeof(String), "secondsParameterName"));
            object[] twoArguments = { 1f, "asd" };
            
            Assert.DoesNotThrow(()=>twoParameters.MapTo(oneParameter, twoArguments));
        }
        
        [Test]
        public void IndirectSubset_Maps()
        {
            ParameterSet oneParameter = new ParameterSet(
                new Parameter(typeof(Single), "firstParameterName"));

            ParameterSet twoParameters = new ParameterSet(
                new Parameter(typeof(String), "secondsParameterName"),
                new Parameter(typeof(Single), "firstParameterName"));
            object[] twoArguments = { "asd", 1f };
            
            Assert.DoesNotThrow(()=>twoParameters.MapTo(oneParameter, twoArguments));
        }
        
        [Test]
        public void DirectAssignable_Maps()
        {
            ParameterSet twoParametersOne = new ParameterSet(
                new Parameter(typeof(String), "secondsParameterNameOne"),
                new Parameter(typeof(Single), "firstParameterNameOne"));
            object[] twoArguments = { "asd", 1f };
            
            ParameterSet twoParametersTwo = new ParameterSet(
                new Parameter(typeof(String), "secondsParameterNameTwo"),
                new Parameter(typeof(Single), "firstParameterNameTwo"));
            
            Assert.DoesNotThrow(()=>twoParametersOne.MapTo(twoParametersTwo, twoArguments));
        }
        
        [Test]
        public void IndirectEqual_Maps()
        {
            ParameterSet twoParametersOne = new ParameterSet(
                new Parameter(typeof(String), "secondsParameterName"),
                new Parameter(typeof(Single), "firstParameterName"));
            object[] twoArguments = { "asd", 1f };

            ParameterSet twoParametersTwo = new ParameterSet(
                new Parameter(typeof(Single), "firstParameterName"),
                new Parameter(typeof(String), "secondsParameterName"));
            
            Assert.DoesNotThrow(()=>twoParametersOne.MapTo(twoParametersTwo, twoArguments));
        }
        
        [Test]
        public void WrongArgumentCount_DontPass()
        {
            object[] emptyArguments = { };
            object[] oneArgument = { "asd" };
            object[] threeArguments = { "asd", 1f, 2 };
            
            ParameterSet twoParametersTwo = new ParameterSet(
                new Parameter(typeof(String), "secondsParameterNameTwo"),
                new Parameter(typeof(Single), "firstParameterNameTwo"));
            
            Assert.IsFalse(twoParametersTwo.AreValidValues(emptyArguments));
            Assert.IsFalse(twoParametersTwo.AreValidValues(oneArgument));
            Assert.IsFalse(twoParametersTwo.AreValidValues(threeArguments));
        }
        
        [Test]
        public void CorrentArguments_Pass()
        {
            object[] twoArguments = { "asd", 1f };
            
            ParameterSet twoParametersTwo = new ParameterSet(
                new Parameter(typeof(String), "secondsParameterNameTwo"),
                new Parameter(typeof(Single), "firstParameterNameTwo"));
            
            Assert.IsTrue(twoParametersTwo.AreValidValues(twoArguments));
        }
        
        [Test]
        public void NonPassing_Throws()
        {
            ParameterSet twoParametersOne = new ParameterSet(
                new Parameter(typeof(String), "secondsParameterNameOne"),
                new Parameter(typeof(Single), "firstParameterNameOne"));
            object[] wrongArguments = { "asd" };
            object[] correntArguments = { "asd", 1f };
            
            ParameterSet twoParametersTwo = new ParameterSet(
                new Parameter(typeof(String), "secondsParameterNameTwo"),
                new Parameter(typeof(Single), "firstParameterNameTwo"));
            
            Assert.IsFalse(twoParametersOne.AreValidValues(wrongArguments));
            Assert.DoesNotThrow(()=>twoParametersOne.MapTo(twoParametersTwo, correntArguments));
            Assert.Throws<ArgumentException>(()=>twoParametersOne.MapTo(twoParametersTwo, wrongArguments));
        }
    }
}