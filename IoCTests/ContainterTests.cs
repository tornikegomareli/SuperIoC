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


        [Test]
        public void ItAllowsAParameterlessConstructor ()
        {
            var subject = (C) Container.GetInstance(typeof(C));

            Assert.IsTrue(subject.Invoked);
        }

        [Test]
        public void ItAllowsGenericInitialization ()
        {
            var subject = Container.GetInstance<A>();
            
            Assert.IsInstanceOf<A>(subject);
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

        internal class C
        {
            public bool? Invoked { get; set; }

            public C ()
            {
                Invoked = true;
            }
        }
       
    }

    [TestFixture]
    public class Container_Register : ContainerTestBase
    {
        [Test]
        public void RegisterATypeFromAnInterface ()
        {
            Container.Register<IMaterial, Plastic>();
            
            var subject = Container.GetInstance<IMaterial>();
            
            Assert.IsInstanceOf<Plastic>(subject);
        }


        [Test]
        public void InitializeObjectWithDependencies ()
        {
            Container.Register<IMaterial, Toy>();

            var subject = (Toy) Container.GetInstance<IMaterial>();
            
            Assert.IsInstanceOf<Plastic>(subject.Material);
        }

        interface IMaterial
        {
            int Weight { get; }
        }

        class Plastic : IMaterial
        {
            public int Weight => 42;
        }

        class Metal : IMaterial
        {
            public int Weight => 84;
        }

        class Toy : IMaterial
        {
            public int Weight => 100;
            
            public Plastic Material { get; set; } = null;

            public Toy(Plastic plasticMaterial)
            {
                Material = plasticMaterial;
            }
        }
    }

    [TestFixture]
    public class Container_RegisterSingleton : ContainerTestBase
    {
        [Test]
        public void ItReturnsASingleInstance ()
        {
            var pet = new Pet(); 
            Container.RegisterSingleton(pet);

            var subject = Container.GetInstance<Pet>();

            Assert.AreEqual(pet, subject);
        }

        class Pet
        {
            
        }
    }
}