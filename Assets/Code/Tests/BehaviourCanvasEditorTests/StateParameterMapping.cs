using System;
using Code.Runtime.BehaviourGraphSerialization;
using NUnit.Framework;

namespace Code.Tests.BehaviourCanvasEditorTests
{
    public class StateParameterMapping
    {
        [Test]
        public void Any_MapsToEmpty()
        {
            Assert.IsTrue(ParameterSet.Empty.CanMapTo(ParameterSet.Empty));
            ParameterSet oneParameter = new ParameterSet(new Parameter(typeof(Single), "parameterName"));
            Assert.IsTrue(oneParameter.CanMapTo(ParameterSet.Empty));
        }

        [Test]
        public void Empty_OnlyMapsToEmpty()
        {
            Assert.IsTrue(ParameterSet.Empty.CanMapTo(ParameterSet.Empty));
            ParameterSet oneParameter = new ParameterSet(new Parameter(typeof(Single), "parameterName"));
            Assert.IsFalse(ParameterSet.Empty.CanMapTo(oneParameter));
        }
        
        [Test]
        public void Superset_DoesntMap()
        {
            ParameterSet oneParameter = new ParameterSet(
                new Parameter(typeof(Single), "firstParameterName"));
            
            ParameterSet twoParameters = new ParameterSet(
                new Parameter(typeof(Single), "firstParameterName"),
                new Parameter(typeof(String), "secondsParameterName"));
            
            Assert.IsFalse(oneParameter.CanMapTo(twoParameters));
        }
        
        [Test]
        public void DirectSubset_Maps()
        {
            ParameterSet oneParameter = new ParameterSet(
                new Parameter(typeof(Single), "firstParameterName"));
            
            ParameterSet twoParameters = new ParameterSet(
                new Parameter(typeof(Single), "firstParameterName"),
                new Parameter(typeof(String), "secondsParameterName"));
            
            Assert.IsTrue(twoParameters.CanMapTo(oneParameter));
        }
        
        [Test]
        public void IndirectSubset_Maps()
        {
            ParameterSet oneParameter = new ParameterSet(
                new Parameter(typeof(Single), "firstParameterName"));

            ParameterSet twoParameters = new ParameterSet(
                new Parameter(typeof(String), "secondsParameterName"),
                new Parameter(typeof(Single), "firstParameterName"));
            
            Assert.IsTrue(twoParameters.CanMapTo(oneParameter));
        }
        
        [Test]
        public void DirectAssignable_Maps()
        {
            ParameterSet twoParametersOne = new ParameterSet(
                new Parameter(typeof(String), "secondsParameterNameOne"),
                new Parameter(typeof(Single), "firstParameterNameOne"));
            
            ParameterSet twoParametersTwo = new ParameterSet(
                new Parameter(typeof(String), "secondsParameterNameTwo"),
                new Parameter(typeof(Single), "firstParameterNameTwo"));
            
            Assert.IsTrue(twoParametersOne.CanMapTo(twoParametersTwo));
        }
        
        [Test]
        public void IndirectEqual_Maps()
        {
            ParameterSet twoParametersOne = new ParameterSet(
                new Parameter(typeof(String), "secondsParameterName"),
                new Parameter(typeof(Single), "firstParameterName"));

            ParameterSet twoParametersTwo = new ParameterSet(
                new Parameter(typeof(Single), "firstParameterName"),
                new Parameter(typeof(String), "secondsParameterName"));
            
            Assert.IsTrue(twoParametersOne.CanMapTo(twoParametersTwo));
        }
    }
}