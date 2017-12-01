﻿using System;
using Flow.Impl.Unity;
using NUnit.Framework;
using UnityEngine;
using System.Linq.Expressions;

namespace Flow.Test
{
    [TestFixture]
    public class MoveToTest
    {
        [Test]
        public void TestMoveTo()
        {
			var k = Create.Kernel<UnityFactory>();
			var f = k.Factory as IUnityFactory;

			//   Vector3 src = Vector3.zero;
			//   Vector3 target = new Vector3(5, 5, 5);
			//var t = MakeRef(ref src);

	        var n = 42;
			//k.Root.Add(f.MoveTo(ref n), 100, 1f));
		}

		[Test]
		public void TestRef()
		{
			//int n = 42;
			//var s = new Storage<int>(ref n);
			//s.Set(12);
			//Assert.AreEqual(12, n);
		}

	    [Test]
	    public void TestExpressionTreeAssign()
	    {
		    var orig = 42;
		    var left = Expression.Variable(typeof(int), "orig");
		    var right = Expression.Constant(12);
		    var assign = Expression.Assign(left, right);

		    Expression block = Expression.Block(
			    new[] { left },
			    assign
		    );

		    Assert.AreEqual(42, orig);
		    orig = Expression.Lambda<Func<int>>(block).Compile()();
		    Assert.AreEqual(12, orig);
	    }
	}
}
