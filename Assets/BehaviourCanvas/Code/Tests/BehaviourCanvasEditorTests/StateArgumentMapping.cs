using System;
using BehaviourCanvas.Code.Runtime.BehaviourGraphSerialization;
using NUnit.Framework;

namespace Code.Tests.BehaviourCanvasEditorTests
{
    public class StateArgumentMapping
    {
        [Test]
        public void Any_MapsToEmpty()
        {
            Assert.That(ParameterSet.Empty.MapTo(ParameterSet.Empty, Array.Empty<object>()), Is.Empty);
            
            ParameterSet oneParameter = new ParameterSet(new Parameter(typeof(Single), "parameterName"));
            object[] oneArgument = { 1f };
            Assert.That(oneParameter.MapTo(ParameterSet.Empty, oneArgument), Is.Empty);
        }

        [Test]
        public void Empty_OnlyMapsToEmpty()
        {
            Assert.That(ParameterSet.Empty.MapTo(ParameterSet.Empty, Array.Empty<object>()), Is.Empty);
            
            ParameterSet oneParameter = new ParameterSet(new Parameter(typeof(Single), "parameterName"));
            Assert.Throws<ArgumentException>(()=>ParameterSet.Empty.MapTo(oneParameter, null));
        }
        
        [Test]
        public void Superset_Throws()
        {
            ParameterSet oneParameter = new ParameterSet(
                new Parameter(typeof(float), "firstParameterName"));
            object[] oneArgument = { 1f };
            
            ParameterSet twoParameters = new ParameterSet(
                new Parameter(typeof(float), "firstParameterName"),
                new Parameter(typeof(float), "secondsParameterName"));
            
            Assert.Throws<ArgumentException>(()=>oneParameter.MapTo(twoParameters, oneArgument));
        }
        
        [Test]
        public void DirectSubset_Maps()
        {
            ParameterSet oneParameter = new ParameterSet(
                new Parameter(typeof(float), "firstParameterName"));
            
            ParameterSet twoParameters = new ParameterSet(
                new Parameter(typeof(float), "firstParameterName"),
                new Parameter(typeof(string), "secondsParameterName"));
            object[] twoArguments = { 1f, "asd" };
            
            Assert.That(twoParameters.MapTo(oneParameter, twoArguments), Is.EquivalentTo(new object[]{1f}));
        }
        
        [Test]
        public void IndirectSubset_Maps()
        {
            ParameterSet oneParameter = new ParameterSet(
                new Parameter(typeof(float), "firstParameterName"));

            ParameterSet twoParameters = new ParameterSet(
                new Parameter(typeof(string), "secondsParameterName"),
                new Parameter(typeof(float), "firstParameterName"));
            object[] twoArguments = { "asd", 1f };
            
            Assert.That(twoParameters.MapTo(oneParameter, twoArguments), Is.EquivalentTo(new object[]{1f}));
        }
        
        [Test]
        public void DirectAssignable_Maps()
        {
            ParameterSet twoParametersOne = new ParameterSet(
                new Parameter(typeof(string), "secondsParameterNameOne"),
                new Parameter(typeof(float), "firstParameterNameOne"));
            object[] twoArguments = { "asd", 1f };
            
            ParameterSet twoParametersTwo = new ParameterSet(
                new Parameter(typeof(string), "secondsParameterNameTwo"),
                new Parameter(typeof(float), "firstParameterNameTwo"));

            Assert.That(twoParametersOne.MapTo(twoParametersTwo, twoArguments), Is.EquivalentTo(twoArguments));
        }
        
        [Test]
        public void IndirectEqual_Maps()
        {
            ParameterSet twoParametersOne = new ParameterSet(
                new Parameter(typeof(string), "secondsParameterName"),
                new Parameter(typeof(float), "firstParameterName"));
            object[] twoArguments = { "asd", 1f };

            ParameterSet twoParametersTwo = new ParameterSet(
                new Parameter(typeof(float), "firstParameterName"),
                new Parameter(typeof(string), "secondsParameterName"));
            
            Assert.That(twoParametersOne.MapTo(twoParametersTwo, twoArguments), Is.EquivalentTo(new object[]{ 1f, "asd" }));
        }
        
        [Test]
        public void WrongArgumentCount_DontPass()
        {
            object[] emptyArguments = { };
            object[] oneArgument = { "asd" };
            object[] threeArguments = { "asd", 1f, 2 };
            
            ParameterSet twoParametersTwo = new ParameterSet(
                new Parameter(typeof(string), "secondsParameterNameTwo"),
                new Parameter(typeof(float), "firstParameterNameTwo"));
            
            Assert.IsFalse(twoParametersTwo.AreValidValues(emptyArguments));
            Assert.IsFalse(twoParametersTwo.AreValidValues(oneArgument));
            Assert.IsTrue(twoParametersTwo.AreValidValues(threeArguments));
        }
        
        [Test]
        public void CorrentArguments_Pass()
        {
            object[] twoArguments = { "asd", 1f };
            
            ParameterSet twoParametersTwo = new ParameterSet(
                new Parameter(typeof(string), "secondsParameterNameTwo"),
                new Parameter(typeof(float), "firstParameterNameTwo"));
            
            Assert.IsTrue(twoParametersTwo.AreValidValues(twoArguments));
        }
        
        [Test]
        public void NonPassing_Throws()
        {
            ParameterSet twoParametersOne = new ParameterSet(
                new Parameter(typeof(string), "secondsParameterNameOne"),
                new Parameter(typeof(float), "firstParameterNameOne"));
            object[] wrongArguments = { "asd" };
            object[] correntArguments = { "asd", 1f };
            
            ParameterSet twoParametersTwo = new ParameterSet(
                new Parameter(typeof(string), "secondsParameterNameTwo"),
                new Parameter(typeof(float), "firstParameterNameTwo"));
            
            Assert.IsFalse(twoParametersOne.AreValidValues(wrongArguments));
            Assert.DoesNotThrow(()=>twoParametersOne.MapTo(twoParametersTwo, correntArguments));
            Assert.Throws<ArgumentException>(()=>twoParametersOne.MapTo(twoParametersTwo, wrongArguments));
        }
    }
}