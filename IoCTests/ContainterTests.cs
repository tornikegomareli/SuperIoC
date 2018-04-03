using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using SuperIoC;
using FluentAssertions;
using System.Xml.Linq;
using System.ComponentModel.Design.Serialization;


namespace IoCTests
{
    
    public class ContainerTestBase
    {
        protected Container Container;

        [SetUp]
        public void BeforeEach ()
        {
            Container = new Container();
        }

        [TearDown]
        public void AfterEach ()
        {
            Container = null;
        }
        
    }

    [TestFixture]
    public class Container_GetInstance : ContainerTestBase
    {
        [Test]
        public void CreatesAnInstanceWithNoParams()
        {
            var subject = (A) Container.GetInstance(typeof(A));

            Assert.IsInstanceOf<A>(subject);
        }


        [Test]
        public void CreatesAnInstanceWithParams()
        {
            var subject = (B) Container.GetInstance(typeof(B));
            
            Assert.IsInstanceOf<A>(subject.A);
        }
        
        internal class A
        {
            
        }

        internal class B
        {
            public A A { get; set; }

            public B(A a)
            {
                A = a;
            }
        }
    }
}