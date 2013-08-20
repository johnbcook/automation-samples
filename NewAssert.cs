// ****************************************************************
// Copyright 2009, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// Modified to allow asserts to return a bool vice throw an exception
// Originates from NUnit version: NUnit-2.5.10.11092


using System;
using System.Collections;
using System.ComponentModel;
using NUnit.Framework.Constraints;

namespace NUnit.Framework
{
    /// <summary>
    /// Delegate used by tests that execute code and
    /// capture any thrown exception.
    /// </summary>
    public delegate void NewAssertTestDelegate();

    /// <summary>
    /// The Assert class contains a collection of static methods that
    /// implement the most common assertions used in NUnit.
    /// </summary>
    public class NewAssert
    {
        #region Constructor

        /// <summary>
        /// We don't actually want any instances of this object, but some people
        /// like to inherit from it to add other static methods. Hence, the
        /// protected constructor disallows any instances of this object. 
        /// </summary>
        protected NewAssert() { }

        #endregion

        #region Assert Counting

        private static int counter = 0;

        /// <summary>
        /// Gets the number of assertions executed so far and 
        /// resets the counter to zero.
        /// </summary>
        public static int Counter
        {
            get
            {
                int cnt = counter;
                counter = 0;
                return cnt;
            }
        }

        private static void IncrementAssertCount()
        {
            ++counter;
        }

        #endregion    

        #region Helper Methods
        /// <summary>
        /// Helper for Assert.AreEqual(double expected, double actual, ...)
        /// allowing code generation to work consistently.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        protected static void AssertDoublesAreEqual(double expected, double actual, double delta, string message, object[] args)
        {
            if (double.IsNaN(expected) || double.IsInfinity(expected))
                Assert.That(actual, Is.EqualTo(expected), message, args);
            else
                Assert.That(actual, Is.EqualTo(expected).Within(delta), message, args);
        }
        #endregion  
   
        #region Utility Asserts

        #region Pass

        /// <summary>
        /// Throws a <see cref="SuccessException"/> with the message and arguments 
        /// that are passed in. This allows a test to be cut short, with a result
        /// of success returned to NUnit.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Pass(string message, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new SuccessException(message);
        }

        /// <summary>
        /// Throws a <see cref="SuccessException"/> with the message and arguments 
        /// that are passed in. This allows a test to be cut short, with a result
        /// of success returned to NUnit.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        static public void Pass(string message)
        {
            Assert.Pass(message, null);
        }

        /// <summary>
        /// Throws a <see cref="SuccessException"/> with the message and arguments 
        /// that are passed in. This allows a test to be cut short, with a result
        /// of success returned to NUnit.
        /// </summary>
        static public void Pass()
        {
            Assert.Pass(string.Empty, null);
        }

        #endregion

        #region Fail

        /// <summary>
        /// Throws an <see cref="AssertionException"/> with the message and arguments 
        /// that are passed in. This is used by the other Assert functions. 
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Fail(string message, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new AssertionException(message);
        }

        /// <summary>
        /// Throws an <see cref="AssertionException"/> with the message that is 
        /// passed in. This is used by the other Assert functions. 
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        static public void Fail(string message)
        {
            Assert.Fail(message, null);
        }

        /// <summary>
        /// Throws an <see cref="AssertionException"/>. 
        /// This is used by the other Assert functions. 
        /// </summary>
        static public void Fail()
        {
            Assert.Fail(string.Empty, null);
        }

        #endregion

        #region Ignore

        /// <summary>
        /// Throws an <see cref="IgnoreException"/> with the message and arguments 
        /// that are passed in.  This causes the test to be reported as ignored.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Ignore(string message, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new IgnoreException(message);
        }

        /// <summary>
        /// Throws an <see cref="IgnoreException"/> with the message that is 
        /// passed in. This causes the test to be reported as ignored. 
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        static public void Ignore(string message)
        {
            Assert.Ignore(message, null);
        }

        /// <summary>
        /// Throws an <see cref="IgnoreException"/>. 
        /// This causes the test to be reported as ignored. 
        /// </summary>
        static public void Ignore()
        {
            Assert.Ignore(string.Empty, null);
        }

        #endregion

        #region InConclusive
        /// <summary>
        /// Throws an <see cref="InconclusiveException"/> with the message and arguments 
        /// that are passed in.  This causes the test to be reported as inconclusive.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="InconclusiveException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Inconclusive(string message, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new InconclusiveException(message);
        }

        /// <summary>
        /// Throws an <see cref="InconclusiveException"/> with the message that is 
        /// passed in. This causes the test to be reported as inconclusive. 
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="InconclusiveException"/> with.</param>
        static public void Inconclusive(string message)
        {
            Assert.Inconclusive(message, null);
        }

        /// <summary>
        /// Throws an <see cref="InconclusiveException"/>. 
        /// This causes the test to be reported as Inconclusive. 
        /// </summary>
        static public void Inconclusive()
        {
            Assert.Inconclusive(string.Empty, null);
        }

        #endregion

        #endregion

        #region NewAssert.That

        #region Object
        /// <summary>
        /// Apply a constraint to an actual value, returns true if the constraint
        /// and false on failure.
        /// </summary>
        /// <param name="expression">A Constraint to be applied</param>
        /// <param name="actual">The actual value to test</param>
        static public void That(object actual, IResolveConstraint expression)
        {
            NewAssert.That(actual, expression, null, null);
        }

        /// <summary>
        /// Apply a constraint to an actual value, returns true if the constraint
        /// is satisfied and false on failure.
        /// </summary>
        /// <param name="expression">A Constraint to be applied</param>
        /// <param name="actual">The actual value to test</param>
        /// <param name="message">The message that will be displayed on failure</param>
        static public void That(object actual, IResolveConstraint expression, string message)
        {
            NewAssert.That(actual, expression, message, null);
        }

        /// <summary>
        /// Apply a constraint to an actual value, returns true if the constraint
        /// is satisfied and false on failure.
        /// </summary>
        /// <param name="expression">A Constraint expression to be applied</param>
        /// <param name="actual">The actual value to test</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public bool That(object actual, IResolveConstraint expression, string message, params object[] args)
        {
            Constraint constraint = expression.Resolve();

            NewAssert.IncrementAssertCount();
            if (!constraint.Matches(actual))
            {
                return false;
            }
            else
            {

                return true;

            }
        }
        #endregion

        #region ActualValueDelegate
        /// <summary>
        /// Apply a constraint to an actual value, returns true if the constraint
        /// is satisfied and false on failure.
        /// </summary>
        /// <param name="expr">A Constraint expression to be applied</param>
        /// <param name="del">An ActualValueDelegate returning the value to be tested</param>
        static public void That(ActualValueDelegate del, IResolveConstraint expr)
        {
            NewAssert.That(del, expr.Resolve(), null, null);
        }

        /// <summary>
        /// Apply a constraint to an actual value, returns true if the constraint
        /// is satisfied and false on failure.
        /// </summary>
        /// <param name="expr">A Constraint expression to be applied</param>
        /// <param name="del">An ActualValueDelegate returning the value to be tested</param>
        /// <param name="message">The message that will be displayed on failure</param>
        static public void That(ActualValueDelegate del, IResolveConstraint expr, string message)
        {
            NewAssert.That(del, expr.Resolve(), message, null);
        }

        /// <summary>
        /// Apply a constraint to an actual value, returns true if the constraint
        /// is satisfied and false on failure.
        /// </summary>
        /// <param name="del">An ActualValueDelegate returning the value to be tested</param>
        /// <param name="expr">A Constraint expression to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public bool That(ActualValueDelegate del, IResolveConstraint expr, string message, params object[] args)
        {
            Constraint constraint = expr.Resolve();

            NewAssert.IncrementAssertCount();
            if (!constraint.Matches(del))
            {
                return false;
            }
            else
            {

                return true;

            }
        }
        #endregion

        #region ref Object
#if NET_2_0
        /// <summary>
        /// Apply a constraint to a referenced value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="expression">A Constraint to be applied</param>
        /// <param name="actual">The actual value to test</param>
        static public void That<T>(ref T actual, IResolveConstraint expression)
        {
            Assert.That(ref actual, expression.Resolve(), null, null);
        }

        /// <summary>
        /// Apply a constraint to a referenced value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="expression">A Constraint to be applied</param>
        /// <param name="actual">The actual value to test</param>
        /// <param name="message">The message that will be displayed on failure</param>
        static public void That<T>(ref T actual, IResolveConstraint expression, string message)
        {
            Assert.That(ref actual, expression.Resolve(), message, null);
        }

        /// <summary>
        /// Apply a constraint to a referenced value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="expression">A Constraint to be applied</param>
        /// <param name="actual">The actual value to test</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void That<T>(ref T actual, IResolveConstraint expression, string message, params object[] args)
        {
            Constraint constraint = expression.Resolve();

            Assert.IncrementAssertCount();
            if (!constraint.Matches(ref actual))
            {
                MessageWriter writer = new TextMessageWriter(message, args);
                constraint.WriteMessageTo(writer);
                throw new AssertionException(writer.ToString());
            }
        }
#else
        /// <summary>
        /// Apply a constraint to a referenced boolean, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="constraint">A Constraint to be applied</param>
        /// <param name="actual">The actual value to test</param>
        static public void That(ref bool actual, IResolveConstraint constraint)
        {
            NewAssert.That(ref actual, constraint.Resolve(), null, null);
        }

        /// <summary>
        /// Apply a constraint to a referenced value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="constraint">A Constraint to be applied</param>
        /// <param name="actual">The actual value to test</param>
        /// <param name="message">The message that will be displayed on failure</param>
        static public void That(ref bool actual, IResolveConstraint constraint, string message)
        {
            NewAssert.That(ref actual, constraint.Resolve(), message, null);
        }

        /// <summary>
        /// Apply a constraint to a referenced value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary> 
        /// <param name="actual">The actual value to test</param>
        /// <param name="expression"></param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public bool That(ref bool actual, IResolveConstraint expression, string message, params object[] args)
        {
            Constraint constraint = expression.Resolve();

            NewAssert.IncrementAssertCount();
            if (!constraint.Matches(ref actual))
            {
                return false;
            }
            else
            {

                return true;

            }
        }
#endif
        #endregion

        #region Boolean
        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary> 
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display if the condition is false</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void That(bool condition, string message, params object[] args)
        {
            Assert.That(condition, Is.True, message, args);
        }

        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display if the condition is false</param>
        static public void That(bool condition, string message)
        {
            Assert.That(condition, Is.True, message, null);
        }

        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        static public void That(bool condition)
        {
            Assert.That(condition, Is.True, null, null);
        }
        #endregion

        /// <summary>
        /// Asserts that the code represented by a delegate throws an exception
        /// that satisfies the constraint provided.
        /// </summary>
        /// <param name="code">A TestDelegate to be executed</param>
        /// <param name="constraint">A ThrowsConstraint used in the test</param>
        static public void That(NewAssertTestDelegate code, IResolveConstraint constraint)
        {
            NewAssert.That((object)code, constraint);
        }
        #endregion
   
        // TODO: Update to NewAssert
        #region True
        
        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static bool True(bool condition, string message, params object[] args)
        {
           bool result = NewAssert.That(condition, Is.True ,message, args);
           return result;
        }
        
        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        public static bool True(bool condition, string message)
        {
            bool result = NewAssert.That(condition, Is.True ,message, null);
            return result;
        }
        
        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        public static bool True(bool condition)
        {
            bool result = NewAssert.That(condition, Is.True ,null, null);
            return result;
        }
        
        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static bool IsTrue(bool condition, string message, params object[] args)
        {
            bool result = NewAssert.That(condition, Is.True ,message, args);
            return result;
        }
        
        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        public static bool IsTrue(bool condition, string message)
        {
            bool result = NewAssert.That(condition, Is.True ,message, null);
            return result;
        }
        
        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        public static bool IsTrue(bool condition)
        {
            bool result = NewAssert.That(condition, Is.True ,null, null);
            return result;
        }
        
        #endregion
        
        // TODO: Update to NewAssert
        #region False
        
        /// <summary>
        /// Asserts that a condition is false. If the condition is true the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary> 
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void False(bool condition, string message, params object[] args)
        {
            NewAssert.That(condition, Is.False ,message, args);
        }
        
        /// <summary>
        /// Asserts that a condition is false. If the condition is true the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary> 
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void False(bool condition, string message)
        {
            NewAssert.That(condition, Is.False ,message, null);
        }
        
        /// <summary>
        /// Asserts that a condition is false. If the condition is true the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary> 
        /// <param name="condition">The evaluated condition</param>
        public static void False(bool condition)
        {
            NewAssert.That(condition, Is.False ,null, null);
        }
        
        /// <summary>
        /// Asserts that a condition is false. If the condition is true the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary> 
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsFalse(bool condition, string message, params object[] args)
        {
            NewAssert.That(condition, Is.False ,message, args);
        }
        
        /// <summary>
        /// Asserts that a condition is false. If the condition is true the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary> 
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void IsFalse(bool condition, string message)
        {
            NewAssert.That(condition, Is.False ,message, null);
        }
        
        /// <summary>
        /// Asserts that a condition is false. If the condition is true the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary> 
        /// <param name="condition">The evaluated condition</param>
        public static void IsFalse(bool condition)
        {
            NewAssert.That(condition, Is.False ,null, null);
        }
        
        #endregion        
        
        #region NotNull
        
        /// <summary>
        /// Verifies that the object that is passed in is not equal to <code>null</code>
        /// If the object is <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static bool NotNull(object anObject, string message, params object[] args)
        {
            bool result = NewAssert.That(anObject, Is.Not.Null ,message, args);
            return result;
        }
        
        /// <summary>
        /// Verifies that the object that is passed in is not equal to <code>null</code>
        /// If the object is <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        public static bool NotNull(object anObject, string message)
        {
           bool result = NewAssert.That(anObject, Is.Not.Null ,message, null);
           return result;
        }
        
        /// <summary>
        /// Verifies that the object that is passed in is not equal to <code>null</code>
        /// If the object is <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        public static bool NotNull(object anObject)
        {
           bool result = NewAssert.That(anObject, Is.Not.Null ,null, null);
           return result;
        }
        
        /// <summary>
        /// Verifies that the object that is passed in is not equal to <code>null</code>
        /// If the object is <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static bool IsNotNull(object anObject, string message, params object[] args)
        {
           bool result = NewAssert.That(anObject, Is.Not.Null ,message, args);
           return result;
        }
        
        /// <summary>
        /// Verifies that the object that is passed in is not equal to <code>null</code>
        /// If the object is <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        public static bool IsNotNull(object anObject, string message)
        {
            bool result = NewAssert.That(anObject, Is.Not.Null ,message, null);
            return result;
        }
        
        /// <summary>
        /// Verifies that the object that is passed in is not equal to <code>null</code>
        /// If the object is <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        public static bool IsNotNull(object anObject)
        {
            bool result = NewAssert.That(anObject, Is.Not.Null ,null, null);
            return result;
        }
        
        #endregion        
        
        #region Null
        
        /// <summary>
        /// Verifies that the object that is passed in is equal to <code>null</code>
        /// If the object is not <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static bool Null(object anObject, string message, params object[] args)
        {
           bool result = NewAssert.That(anObject, Is.Null ,message, args);
           return result;
        }
        
        /// <summary>
        /// Verifies that the object that is passed in is equal to <code>null</code>
        /// If the object is not <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        public static bool Null(object anObject, string message)
        {
            bool result = NewAssert.That(anObject, Is.Null ,message, null);
            return result;
        }
        
        /// <summary>
        /// Verifies that the object that is passed in is equal to <code>null</code>
        /// If the object is not <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        public static bool Null(object anObject)
        {
            bool result = NewAssert.That(anObject, Is.Null ,null, null);
            return result;
        }
        
        /// <summary>
        /// Verifies that the object that is passed in is equal to <code>null</code>
        /// If the object is not <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static bool IsNull(object anObject, string message, params object[] args)
        {
            bool result = NewAssert.That(anObject, Is.Null ,message, args);
            return result;
        }
        
        /// <summary>
        /// Verifies that the object that is passed in is equal to <code>null</code>
        /// If the object is not <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        public static bool IsNull(object anObject, string message)
        {
           bool result = NewAssert.That(anObject, Is.Null ,message, null);
           return result;
        }
        
        /// <summary>
        /// Verifies that the object that is passed in is equal to <code>null</code>
        /// If the object is not <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        public static bool IsNull(object anObject)
        {
           bool result = NewAssert.That(anObject, Is.Null ,null, null);
           return result;
        }
        
        #endregion
        
        // TODO: Update to NewAssert
        #region IsNaN
        
        /// <summary>
        /// Verifies that the double that is passed in is an <code>NaN</code> value.
        /// If the object is not <code>NaN</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="aDouble">The value that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNaN(double aDouble, string message, params object[] args)
        {
            NewAssert.That(aDouble, Is.NaN ,message, args);
        }
        
        /// <summary>
        /// Verifies that the double that is passed in is an <code>NaN</code> value.
        /// If the object is not <code>NaN</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="aDouble">The value that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void IsNaN(double aDouble, string message)
        {
            NewAssert.That(aDouble, Is.NaN ,message, null);
        }
        
        /// <summary>
        /// Verifies that the double that is passed in is an <code>NaN</code> value.
        /// If the object is not <code>NaN</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="aDouble">The value that is to be tested</param>
        public static void IsNaN(double aDouble)
        {
            NewAssert.That(aDouble, Is.NaN ,null, null);
        }
        
#if NET_2_0
        /// <summary>
        /// Verifies that the double that is passed in is an <code>NaN</code> value.
        /// If the object is not <code>NaN</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="aDouble">The value that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNaN(double? aDouble, string message, params object[] args)
        {
            Assert.That(aDouble, Is.NaN ,message, args);
        }
        
        /// <summary>
        /// Verifies that the double that is passed in is an <code>NaN</code> value.
        /// If the object is not <code>NaN</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="aDouble">The value that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void IsNaN(double? aDouble, string message)
        {
            Assert.That(aDouble, Is.NaN ,message, null);
        }
        
        /// <summary>
        /// Verifies that the double that is passed in is an <code>NaN</code> value.
        /// If the object is not <code>NaN</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="aDouble">The value that is to be tested</param>
        public static void IsNaN(double? aDouble)
        {
            Assert.That(aDouble, Is.NaN ,null, null);
        }
        
#endif
        #endregion
        
        // TODO: Update to NewAssert
        #region IsEmpty
        
        /// <summary>
        /// Assert that a string is empty - that is equal to string.Empty
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsEmpty(string aString, string message, params object[] args)
        {
            NewAssert.That(aString, new EmptyStringConstraint() ,message, args);
        }
        
        /// <summary>
        /// Assert that a string is empty - that is equal to string.Empty
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void IsEmpty(string aString, string message)
        {
            NewAssert.That(aString, new EmptyStringConstraint() ,message, null);
        }
        
        /// <summary>
        /// Assert that a string is empty - that is equal to string.Empty
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        public static void IsEmpty(string aString)
        {
            NewAssert.That(aString, new EmptyStringConstraint() ,null, null);
        }
        
        #endregion

        // TODO: Update to NewAssert
        #region IsEmpty
        
        /// <summary>
        /// Assert that an array, list or other collection is empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsEmpty(ICollection collection, string message, params object[] args)
        {
            NewAssert.That(collection, new EmptyCollectionConstraint() ,message, args);
        }
        
        /// <summary>
        /// Assert that an array, list or other collection is empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void IsEmpty(ICollection collection, string message)
        {
            NewAssert.That(collection, new EmptyCollectionConstraint() ,message, null);
        }
        
        /// <summary>
        /// Assert that an array, list or other collection is empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        public static void IsEmpty(ICollection collection)
        {
            NewAssert.That(collection, new EmptyCollectionConstraint() ,null, null);
        }
        
        #endregion

        // TODO: Update to NewAssert
        #region IsNotEmpty
        
        /// <summary>
        /// Assert that a string is not empty - that is not equal to string.Empty
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNotEmpty(string aString, string message, params object[] args)
        {
            NewAssert.That(aString, Is.Not.Empty ,message, args);
        }
        
        /// <summary>
        /// Assert that a string is not empty - that is not equal to string.Empty
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void IsNotEmpty(string aString, string message)
        {
            NewAssert.That(aString, Is.Not.Empty ,message, null);
        }
        
        /// <summary>
        /// Assert that a string is not empty - that is not equal to string.Empty
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        public static void IsNotEmpty(string aString)
        {
            NewAssert.That(aString, Is.Not.Empty ,null, null);
        }
        
        #endregion

       
        #region IsNotEmpty
        
        /// <summary>
        /// Assert that an array, list or other collection is not empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNotEmpty(ICollection collection, string message, params object[] args)
        {
           NewAssert.That(collection, Is.Not.Empty ,message, args);
        }
        
        /// <summary>
        /// Assert that an array, list or other collection is not empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void IsNotEmpty(ICollection collection, string message)
        {
            NewAssert.That(collection, Is.Not.Empty ,message, null);
        }
        
        /// <summary>
        /// Assert that an array, list or other collection is not empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        public static void IsNotEmpty(ICollection collection)
        {
           NewAssert.That(collection, Is.Not.Empty ,null, null);
        }
        
        #endregion

       
        #region IsNullOrEmpty
        
        /// <summary>
        /// Assert that a string is either null or equal to string.Empty
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static bool IsNullOrEmpty(string aString, string message, params object[] args)
        {
            bool result = NewAssert.That(aString, new NullOrEmptyStringConstraint(), message, args);
            return result;
        }
        
        /// <summary>
        /// Assert that a string is either null or equal to string.Empty
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        public static bool IsNullOrEmpty(string aString, string message)
        {
            bool result = NewAssert.That(aString, new NullOrEmptyStringConstraint(), message, null);
            return result;
        }
        
        /// <summary>
        /// Assert that a string is either null or equal to string.Empty
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        public static bool IsNullOrEmpty(string aString)
        {
            bool result = NewAssert.That(aString, new NullOrEmptyStringConstraint(), null, null);
            return result;
        }
        
        #endregion

        
        #region IsNotNullOrEmpty
        
        /// <summary>
        /// Assert that a string is not null or empty
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static bool IsNotNullOrEmpty(string aString, string message, params object[] args)
        {
            bool result = NewAssert.That(aString, new NotConstraint(new NullOrEmptyStringConstraint()), message, args);
            return result;
        }
        
        /// <summary>
        /// Assert that a string is not null or empty
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        public static bool IsNotNullOrEmpty(string aString, string message)
        {
            bool result = NewAssert.That(aString, new NotConstraint(new NullOrEmptyStringConstraint()), message, null);
            return result;
        }
        
        /// <summary>
        /// Assert that a string is not null or empty
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        public static bool IsNotNullOrEmpty(string aString)
        {
            bool result = NewAssert.That(aString, new NotConstraint(new NullOrEmptyStringConstraint()), null, null);
            return result;
        }
        
        #endregion
        
        #region IsAssignableFrom
        
        /// <summary>
        /// Asserts that an object may be assigned a  value of a given Type.
        /// </summary>
        /// <param name="expected">The expected Type.</param>
        /// <param name="actual">The object under examination</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsAssignableFrom(Type expected, object actual, string message, params object[] args)
        {
            NewAssert.That(actual, Is.AssignableFrom(expected) ,message, args);
        }
        
        /// <summary>
        /// Asserts that an object may be assigned a  value of a given Type.
        /// </summary>
        /// <param name="expected">The expected Type.</param>
        /// <param name="actual">The object under examination</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void IsAssignableFrom(Type expected, object actual, string message)
        {
            NewAssert.That(actual, Is.AssignableFrom(expected) ,message, null);
        }
        
        /// <summary>
        /// Asserts that an object may be assigned a  value of a given Type.
        /// </summary>
        /// <param name="expected">The expected Type.</param>
        /// <param name="actual">The object under examination</param>
        public static void IsAssignableFrom(Type expected, object actual)
        {
            NewAssert.That(actual, Is.AssignableFrom(expected) ,null, null);
        }
        
        #endregion

        #region IsAssignableFrom<T>
        
#if NET_2_0
        /// <summary>
        /// Asserts that an object may be assigned a  value of a given Type.
        /// </summary>
        /// <typeparam name="T">The expected Type.</typeparam>
        /// <param name="actual">The object under examination</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsAssignableFrom<T>(object actual, string message, params object[] args)
        {
            Assert.That(actual, Is.AssignableFrom(typeof(T)) ,message, args);
        }
        
        /// <summary>
        /// Asserts that an object may be assigned a  value of a given Type.
        /// </summary>
        /// <typeparam name="T">The expected Type.</typeparam>
        /// <param name="actual">The object under examination</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void IsAssignableFrom<T>(object actual, string message)
        {
            Assert.That(actual, Is.AssignableFrom(typeof(T)) ,message, null);
        }
        
        /// <summary>
        /// Asserts that an object may be assigned a  value of a given Type.
        /// </summary>
        /// <typeparam name="T">The expected Type.</typeparam>
        /// <param name="actual">The object under examination</param>
        public static void IsAssignableFrom<T>(object actual)
        {
            Assert.That(actual, Is.AssignableFrom(typeof(T)) ,null, null);
        }
        
#endif
        #endregion

        
        #region IsNotAssignableFrom
        
        /// <summary>
        /// Asserts that an object may not be assigned a  value of a given Type.
        /// </summary>
        /// <param name="expected">The expected Type.</param>
        /// <param name="actual">The object under examination</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNotAssignableFrom(Type expected, object actual, string message, params object[] args)
        {
            NewAssert.That(actual, Is.Not.AssignableFrom(expected) ,message, args);
        }
        
        /// <summary>
        /// Asserts that an object may not be assigned a  value of a given Type.
        /// </summary>
        /// <param name="expected">The expected Type.</param>
        /// <param name="actual">The object under examination</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void IsNotAssignableFrom(Type expected, object actual, string message)
        {
            NewAssert.That(actual, Is.Not.AssignableFrom(expected) ,message, null);
        }
        
        /// <summary>
        /// Asserts that an object may not be assigned a  value of a given Type.
        /// </summary>
        /// <param name="expected">The expected Type.</param>
        /// <param name="actual">The object under examination</param>
        public static void IsNotAssignableFrom(Type expected, object actual)
        {
            NewAssert.That(actual, Is.Not.AssignableFrom(expected) ,null, null);
        }
        
        #endregion

       
        #region IsNotAssignableFrom<T>
        
#if NET_2_0
        /// <summary>
        /// Asserts that an object may not be assigned a  value of a given Type.
        /// </summary>
        /// <typeparam name="T">The expected Type.</typeparam>
        /// <param name="actual">The object under examination</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNotAssignableFrom<T>(object actual, string message, params object[] args)
        {
            Assert.That(actual, Is.Not.AssignableFrom(typeof(T)) ,message, args);
        }
        
        /// <summary>
        /// Asserts that an object may not be assigned a  value of a given Type.
        /// </summary>
        /// <typeparam name="T">The expected Type.</typeparam>
        /// <param name="actual">The object under examination</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void IsNotAssignableFrom<T>(object actual, string message)
        {
            Assert.That(actual, Is.Not.AssignableFrom(typeof(T)) ,message, null);
        }
        
        /// <summary>
        /// Asserts that an object may not be assigned a  value of a given Type.
        /// </summary>
        /// <typeparam name="T">The expected Type.</typeparam>
        /// <param name="actual">The object under examination</param>
        public static void IsNotAssignableFrom<T>(object actual)
        {
            Assert.That(actual, Is.Not.AssignableFrom(typeof(T)) ,null, null);
        }
        
#endif
        #endregion
        
        #region IsInstanceOf
        
        /// <summary>
        /// Asserts that an object is an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsInstanceOf(Type expected, object actual, string message, params object[] args)
        {
            NewAssert.That(actual, Is.InstanceOf(expected) ,message, args);
        }
        
        /// <summary>
        /// Asserts that an object is an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void IsInstanceOf(Type expected, object actual, string message)
        {
            NewAssert.That(actual, Is.InstanceOf(expected) ,message, null);
        }
        
        /// <summary>
        /// Asserts that an object is an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        public static void IsInstanceOf(Type expected, object actual)
        {
            NewAssert.That(actual, Is.InstanceOf(expected) ,null, null);
        }
        
        /// <summary>
        /// Asserts that an object is an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        [Obsolete]
        public static void IsInstanceOfType(Type expected, object actual, string message, params object[] args)
        {
            Assert.That(actual, Is.InstanceOf(expected) ,message, args);
        }
        
        /// <summary>
        /// Asserts that an object is an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">The message to display in case of failure</param>
        [Obsolete]
        public static void IsInstanceOfType(Type expected, object actual, string message)
        {
            Assert.That(actual, Is.InstanceOf(expected) ,message, null);
        }
        
        /// <summary>
        /// Asserts that an object is an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        [Obsolete]
        public static void IsInstanceOfType(Type expected, object actual)
        {
            Assert.That(actual, Is.InstanceOf(expected) ,null, null);
        }
        
        #endregion
        
        #region IsInstanceOf<T>
        
#if NET_2_0
        /// <summary>
        /// Asserts that an object is an instance of a given type.
        /// </summary>
        /// <typeparam name="T">The expected Type</typeparam>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsInstanceOf<T>(object actual, string message, params object[] args)
        {
            Assert.That(actual, Is.InstanceOf(typeof(T)) ,message, args);
        }
        
        /// <summary>
        /// Asserts that an object is an instance of a given type.
        /// </summary>
        /// <typeparam name="T">The expected Type</typeparam>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void IsInstanceOf<T>(object actual, string message)
        {
            Assert.That(actual, Is.InstanceOf(typeof(T)) ,message, null);
        }
        
        /// <summary>
        /// Asserts that an object is an instance of a given type.
        /// </summary>
        /// <typeparam name="T">The expected Type</typeparam>
        /// <param name="actual">The object being examined</param>
        public static void IsInstanceOf<T>(object actual)
        {
            Assert.That(actual, Is.InstanceOf(typeof(T)) ,null, null);
        }
        
#endif
        #endregion

        // TODO: Update to NewAssert
        #region IsNotInstanceOf
        
        /// <summary>
        /// Asserts that an object is not an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNotInstanceOf(Type expected, object actual, string message, params object[] args)
        {
            NewAssert.That(actual, Is.Not.InstanceOf(expected) ,message, args);
        }
        
        /// <summary>
        /// Asserts that an object is not an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void IsNotInstanceOf(Type expected, object actual, string message)
        {
            NewAssert.That(actual, Is.Not.InstanceOf(expected) ,message, null);
        }
        
        /// <summary>
        /// Asserts that an object is not an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        public static void IsNotInstanceOf(Type expected, object actual)
        {
            NewAssert.That(actual, Is.Not.InstanceOf(expected) ,null, null);
        }
        
        /// <summary>
        /// Asserts that an object is not an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        [Obsolete]
        public static void IsNotInstanceOfType(Type expected, object actual, string message, params object[] args)
        {
            NewAssert.That(actual, Is.Not.InstanceOf(expected) ,message, args);
        }
        
        /// <summary>
        /// Asserts that an object is not an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">The message to display in case of failure</param>
        [Obsolete]
        public static void IsNotInstanceOfType(Type expected, object actual, string message)
        {
           NewAssert.That(actual, Is.Not.InstanceOf(expected) ,message, null);
        }
        
        /// <summary>
        /// Asserts that an object is not an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        [Obsolete]
        public static void IsNotInstanceOfType(Type expected, object actual)
        {
            NewAssert.That(actual, Is.Not.InstanceOf(expected) ,null, null);
        }
        
        #endregion

        #region IsNotInstanceOf<T>
        
#if NET_2_0
        /// <summary>
        /// Asserts that an object is not an instance of a given type.
        /// </summary>
        /// <typeparam name="T">The expected Type</typeparam>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNotInstanceOf<T>(object actual, string message, params object[] args)
        {
            Assert.That(actual, Is.Not.InstanceOf(typeof(T)) ,message, args);
        }
        
        /// <summary>
        /// Asserts that an object is not an instance of a given type.
        /// </summary>
        /// <typeparam name="T">The expected Type</typeparam>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void IsNotInstanceOf<T>(object actual, string message)
        {
            Assert.That(actual, Is.Not.InstanceOf(typeof(T)) ,message, null);
        }
        
        /// <summary>
        /// Asserts that an object is not an instance of a given type.
        /// </summary>
        /// <typeparam name="T">The expected Type</typeparam>
        /// <param name="actual">The object being examined</param>
        public static void IsNotInstanceOf<T>(object actual)
        {
            Assert.That(actual, Is.Not.InstanceOf(typeof(T)) ,null, null);
        }
        
#endif
        #endregion
        
        #region AreEqual
        
        /// <summary>
        /// Verifies that two values are equal. Return true on Equal false on !Equal 
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">fail message</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static bool AreEqual(int expected, int actual, string message, params object[] args)
        {
           bool result =  NewAssert.That(actual, Is.EqualTo(expected) ,message, args);
           return result;
        }
        
        /// <summary>
        /// Verifies that two values are equal. Return true on Equal false on !Equal         
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        public static bool AreEqual(int expected, int actual, string message)
        {
            bool result = NewAssert.That(actual, Is.EqualTo(expected) ,message, null);
            return result;
        }
        
        /// <summary>
        /// Verifies that two values are equal. Return true on Equal false on !Equal         
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        public static bool AreEqual(int expected, int actual)
        {
           bool result =   NewAssert.That(actual, Is.EqualTo(expected) ,null, null);
           return result;
        }
        
        /// <summary>
        ///Verifies that two values are equal. Return true on Equal false on !Equal 
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static bool AreEqual(long expected, long actual, string message, params object[] args)
        {
            bool result = NewAssert.That(actual, Is.EqualTo(expected) ,message, args);
            return result;
        }
        
        /// <summary>
        /// Verifies that two values are equal. Return true on Equal false on !Equal        
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        public static bool AreEqual(long expected, long actual, string message)
        {
            bool result = NewAssert.That(actual, Is.EqualTo(expected) ,message, null);
            return result;
        }
        
        /// <summary>
        /// Verifies that two values are equal. Return true on Equal false on !Equal 
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        public static bool AreEqual(long expected, long actual)
        {
            bool result = NewAssert.That(actual, Is.EqualTo(expected) ,null, null);
            return result;
        }
        
        /// <summary>
        /// Verifies that two values are equal. Return true on Equal false on !Equal        
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        
        public static bool AreEqual(uint expected, uint actual, string message, params object[] args)
        {
            bool result = NewAssert.That(actual, Is.EqualTo(expected) ,message, args);
            return result;
        }
        
        /// <summary>
        /// Verifies that two values are equal. Return true on Equal false on !Equal        
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
       
        public static bool AreEqual(uint expected, uint actual, string message)
        {
            bool result = NewAssert.That(actual, Is.EqualTo(expected) ,message, null);
            return result;
        }
        
        /// <summary>
        /// Verifies that two values are equal. Return true on Equal false on !Equal 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
       
        public static bool AreEqual(uint expected, uint actual)
        {
           bool result = NewAssert.That(actual, Is.EqualTo(expected) ,null, null);
           return result;
        }
        
        /// <summary>
        ///Verifies that two values are equal. Return true on Equal false on !Equal        
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
       
        public static bool AreEqual(ulong expected, ulong actual, string message, params object[] args)
        {
            bool result = NewAssert.That(actual, Is.EqualTo(expected) ,message, args);
            return result;
            
        }
        
        /// <summary>
        /// Verifies that two values are equal. Return true on Equal false on !Equal        
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        
        public static bool AreEqual(ulong expected, ulong actual, string message)
        {
            bool result = NewAssert.That(actual, Is.EqualTo(expected) ,message, null);
            return result;
        }
        
        /// <summary>
        /// Verifies that two values are equal. Return true on Equal false on !Equal 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
       
        public static bool AreEqual(ulong expected, ulong actual)
        {
           bool result =  NewAssert.That(actual, Is.EqualTo(expected) ,null, null);
           return result;
           
        }
        
        /// <summary>
        /// Verifies that two values are equal. Return true on Equal false on !Equal        
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static bool AreEqual(decimal expected, decimal actual, string message, params object[] args)
        {
           bool result =  NewAssert.That(actual, Is.EqualTo(expected) ,message, args);
           return result;
        }
        
        /// <summary>
        /// Verifies that two values are equal. Return true on Equal false on !Equal        
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        public static bool AreEqual(decimal expected, decimal actual, string message)
        {
            bool result = NewAssert.That(actual, Is.EqualTo(expected) ,message, null);
            return result;
        }
        
        /// <summary>
        /// Verifies that two values are equal. Return true on Equal false on !Equal         
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        public static bool AreEqual(decimal expected, decimal actual)
        {
            bool result = NewAssert.That(actual, Is.EqualTo(expected) ,null, null);
            return result;
        }
        
        #endregion
        
        #region AreEqual
        
        /// <summary>
        /// Verifies that two doubles are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If 
        /// they are not equal then an <see cref="AssertionException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual(double expected, double actual, double delta, string message, params object[] args)
        {
            AssertDoublesAreEqual(expected, actual, delta ,message, args);
        }
        
        /// <summary>
        /// Verifies that two doubles are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If 
        /// they are not equal then an <see cref="AssertionException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void AreEqual(double expected, double actual, double delta, string message)
        {
            AssertDoublesAreEqual(expected, actual, delta ,message, null);
        }
        
        /// <summary>
        /// Verifies that two doubles are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If 
        /// they are not equal then an <see cref="AssertionException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        public static void AreEqual(double expected, double actual, double delta)
        {
            AssertDoublesAreEqual(expected, actual, delta ,null, null);
        }
        
#if NET_2_0
        /// <summary>
        /// Verifies that two doubles are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If 
        /// they are not equal then an <see cref="AssertionException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual(double expected, double? actual, double delta, string message, params object[] args)
        {
            AssertDoublesAreEqual(expected, (double)actual, delta ,message, args);
        }
        
        /// <summary>
        /// Verifies that two doubles are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If 
        /// they are not equal then an <see cref="AssertionException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void AreEqual(double expected, double? actual, double delta, string message)
        {
            AssertDoublesAreEqual(expected, (double)actual, delta ,message, null);
        }
        
        /// <summary>
        /// Verifies that two doubles are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If 
        /// they are not equal then an <see cref="AssertionException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        public static void AreEqual(double expected, double? actual, double delta)
        {
            AssertDoublesAreEqual(expected, (double)actual, delta ,null, null);
        }
        
#endif
        #endregion
        
        #region AreEqual
        
        /// <summary>
        /// Verifies that two objects are equal.  Two objects are considered
        /// equal if both are null, or if both have the same value. NUnit
        /// has special semantics for some object types.
        /// If they are not equal false is returned
        /// </summary>
        /// <param name="expected">The value that is expected</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static bool AreEqual(object expected, object actual, string message, params object[] args)
        {
            bool result = NewAssert.That(actual, Is.EqualTo(expected) ,message, args);
            return result;
        }
        
        /// <summary>
        /// Verifies that two objects are equal.  Two objects are considered
        /// equal if both are null, or if both have the same value. NUnit
        /// has special semantics for some object types.
        /// If they are not equal an false is returned.
        /// </summary>
        /// <param name="expected">The value that is expected</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        public static bool AreEqual(object expected, object actual, string message)
        {
            bool result = NewAssert.That(actual, Is.EqualTo(expected) ,message, null);
            return result;
        }
        
        /// <summary>
        /// Verifies that two objects are equal.  Two objects are considered
        /// equal if both are null, or if both have the same value. NUnit
        /// has special semantics for some object types.
        /// If they are not equal false is returned.
        /// </summary>
        /// <param name="expected">The value that is expected</param>
        /// <param name="actual">The actual value</param>
        public static bool AreEqual(object expected, object actual)
        {
           bool result = NewAssert.That(actual, Is.EqualTo(expected) ,null, null);
           return result;
        }
        
        #endregion
        //TODO - Add NewAssert
        #region AreNotEqual
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotEqual(int expected, int actual, string message, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,message, args);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void AreNotEqual(int expected, int actual, string message)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,message, null);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        public static void AreNotEqual(int expected, int actual)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,null, null);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotEqual(long expected, long actual, string message, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,message, args);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void AreNotEqual(long expected, long actual, string message)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,message, null);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        public static void AreNotEqual(long expected, long actual)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,null, null);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
       
        public static void AreNotEqual(uint expected, uint actual, string message, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,message, args);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
      
        public static void AreNotEqual(uint expected, uint actual, string message)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,message, null);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
       
        public static void AreNotEqual(uint expected, uint actual)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,null, null);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
      
        public static void AreNotEqual(ulong expected, ulong actual, string message, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,message, args);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
       
        public static void AreNotEqual(ulong expected, ulong actual, string message)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,message, null);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
       
        public static void AreNotEqual(ulong expected, ulong actual)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,null, null);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotEqual(decimal expected, decimal actual, string message, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,message, args);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void AreNotEqual(decimal expected, decimal actual, string message)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,message, null);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        public static void AreNotEqual(decimal expected, decimal actual)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,null, null);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotEqual(float expected, float actual, string message, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,message, args);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void AreNotEqual(float expected, float actual, string message)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,message, null);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        public static void AreNotEqual(float expected, float actual)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,null, null);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotEqual(double expected, double actual, string message, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,message, args);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void AreNotEqual(double expected, double actual, string message)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,message, null);
        }
        
        /// <summary>
        /// Verifies that two values are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        public static void AreNotEqual(double expected, double actual)
        {
            Assert.That(actual, Is.Not.EqualTo(expected) ,null, null);
        }
        
        #endregion
        //TODO - Add NewAssert
        #region AreNotEqual
        
        /// <summary>
        /// Verifies that two objects are not equal.  Two objects are considered
        /// equal if both are null, or if both have the same value. NUnit
        /// has special semantics for some object types.
        /// If they are equal an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The value that is expected</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static bool AreNotEqual(object expected, object actual, string message, params object[] args)
        {
           bool result =  NewAssert.That(actual, Is.Not.EqualTo(expected) ,message, args);
           return result;
        }
        
        /// <summary>
        /// Verifies that two objects are not equal.  Two objects are considered
        /// equal if both are null, or if both have the same value. NUnit
        /// has special semantics for some object types.
        /// If they are equal an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The value that is expected</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        public static bool AreNotEqual(object expected, object actual, string message)
        {
            bool result = NewAssert.That(actual, Is.Not.EqualTo(expected) ,message, null);
            return result;
        }
        
        /// <summary>
        /// Verifies that two objects are not equal.  Two objects are considered
        /// equal if both are null, or if both have the same value. NUnit
        /// has special semantics for some object types.
        /// If they are equal an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The value that is expected</param>
        /// <param name="actual">The actual value</param>
        public static bool AreNotEqual(object expected, object actual)
        {
           bool result =  NewAssert.That(actual, Is.Not.EqualTo(expected) ,null, null);
           return result;
        }
        
        #endregion
        //TODO - Add NewAssert
        #region AreSame
        
        /// <summary>
        /// Asserts that two objects refer to the same object. If they
        /// are not the same an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreSame(object expected, object actual, string message, params object[] args)
        {
            Assert.That(actual, Is.SameAs(expected) ,message, args);
        }
        
        /// <summary>
        /// Asserts that two objects refer to the same object. If they
        /// are not the same an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void AreSame(object expected, object actual, string message)
        {
            Assert.That(actual, Is.SameAs(expected) ,message, null);
        }
        
        /// <summary>
        /// Asserts that two objects refer to the same object. If they
        /// are not the same an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        public static void AreSame(object expected, object actual)
        {
            Assert.That(actual, Is.SameAs(expected) ,null, null);
        }
        
        #endregion
        //TODO - Add NewAssert
        #region AreNotSame
        
        /// <summary>
        /// Asserts that two objects do not refer to the same object. If they
        /// are the same an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotSame(object expected, object actual, string message, params object[] args)
        {
            Assert.That(actual, Is.Not.SameAs(expected) ,message, args);
        }
        
        /// <summary>
        /// Asserts that two objects do not refer to the same object. If they
        /// are the same an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void AreNotSame(object expected, object actual, string message)
        {
            Assert.That(actual, Is.Not.SameAs(expected) ,message, null);
        }
        
        /// <summary>
        /// Asserts that two objects do not refer to the same object. If they
        /// are the same an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        public static void AreNotSame(object expected, object actual)
        {
            Assert.That(actual, Is.Not.SameAs(expected) ,null, null);
        }
        
        #endregion
        //TODO - Add NewAssert
        #region Greater
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Greater(int arg1, int arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void Greater(int arg1, int arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        public static void Greater(int arg1, int arg2)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
     
        public static void Greater(uint arg1, uint arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        
        public static void Greater(uint arg1, uint arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
      
        public static void Greater(uint arg1, uint arg2)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Greater(long arg1, long arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void Greater(long arg1, long arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        public static void Greater(long arg1, long arg2)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
       
        public static void Greater(ulong arg1, ulong arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
       
        public static void Greater(ulong arg1, ulong arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
       
        public static void Greater(ulong arg1, ulong arg2)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Greater(decimal arg1, decimal arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void Greater(decimal arg1, decimal arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        public static void Greater(decimal arg1, decimal arg2)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Greater(double arg1, double arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void Greater(double arg1, double arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        public static void Greater(double arg1, double arg2)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Greater(float arg1, float arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void Greater(float arg1, float arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        public static void Greater(float arg1, float arg2)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Greater(IComparable arg1, IComparable arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void Greater(IComparable arg1, IComparable arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        public static void Greater(IComparable arg1, IComparable arg2)
        {
            Assert.That(arg1, Is.GreaterThan(arg2) ,null, null);
        }
        
        #endregion
        //TODO - Add NewAssert
        #region Less
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Less(int arg1, int arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void Less(int arg1, int arg2, string message)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        public static void Less(int arg1, int arg2)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
       
        public static void Less(uint arg1, uint arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
       
        public static void Less(uint arg1, uint arg2, string message)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
       
        public static void Less(uint arg1, uint arg2)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Less(long arg1, long arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void Less(long arg1, long arg2, string message)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        public static void Less(long arg1, long arg2)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
       
        public static void Less(ulong arg1, ulong arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
       
        public static void Less(ulong arg1, ulong arg2, string message)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
       
        public static void Less(ulong arg1, ulong arg2)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Less(decimal arg1, decimal arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void Less(decimal arg1, decimal arg2, string message)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        public static void Less(decimal arg1, decimal arg2)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Less(double arg1, double arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void Less(double arg1, double arg2, string message)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        public static void Less(double arg1, double arg2)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Less(float arg1, float arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void Less(float arg1, float arg2, string message)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        public static void Less(float arg1, float arg2)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Less(IComparable arg1, IComparable arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void Less(IComparable arg1, IComparable arg2, string message)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        public static void Less(IComparable arg1, IComparable arg2)
        {
            Assert.That(arg1, Is.LessThan(arg2) ,null, null);
        }
        
        #endregion
        //TODO - Add NewAssert

        #region GreaterOrEqual
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void GreaterOrEqual(int arg1, int arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void GreaterOrEqual(int arg1, int arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        public static void GreaterOrEqual(int arg1, int arg2)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
       
        public static void GreaterOrEqual(uint arg1, uint arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
       
        public static void GreaterOrEqual(uint arg1, uint arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
       
        public static void GreaterOrEqual(uint arg1, uint arg2)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void GreaterOrEqual(long arg1, long arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void GreaterOrEqual(long arg1, long arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        public static void GreaterOrEqual(long arg1, long arg2)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
       
        public static void GreaterOrEqual(ulong arg1, ulong arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
       
        public static void GreaterOrEqual(ulong arg1, ulong arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
       
        public static void GreaterOrEqual(ulong arg1, ulong arg2)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void GreaterOrEqual(decimal arg1, decimal arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void GreaterOrEqual(decimal arg1, decimal arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        public static void GreaterOrEqual(decimal arg1, decimal arg2)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void GreaterOrEqual(double arg1, double arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void GreaterOrEqual(double arg1, double arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        public static void GreaterOrEqual(double arg1, double arg2)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void GreaterOrEqual(float arg1, float arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void GreaterOrEqual(float arg1, float arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        public static void GreaterOrEqual(float arg1, float arg2)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void GreaterOrEqual(IComparable arg1, IComparable arg2, string message, params object[] args)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void GreaterOrEqual(IComparable arg1, IComparable arg2, string message)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is greater than or equal tothe second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be greater</param>
        /// <param name="arg2">The second value, expected to be less</param>
        public static void GreaterOrEqual(IComparable arg1, IComparable arg2)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2) ,null, null);
        }
        
        #endregion
        
        #region LessOrEqual
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void LessOrEqual(int arg1, int arg2, string message, params object[] args)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void LessOrEqual(int arg1, int arg2, string message)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        public static void LessOrEqual(int arg1, int arg2)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
       
        public static void LessOrEqual(uint arg1, uint arg2, string message, params object[] args)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
       
        public static void LessOrEqual(uint arg1, uint arg2, string message)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
       
        public static void LessOrEqual(uint arg1, uint arg2)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void LessOrEqual(long arg1, long arg2, string message, params object[] args)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void LessOrEqual(long arg1, long arg2, string message)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        public static void LessOrEqual(long arg1, long arg2)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
       
        public static void LessOrEqual(ulong arg1, ulong arg2, string message, params object[] args)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
       
        public static void LessOrEqual(ulong arg1, ulong arg2, string message)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
       
        public static void LessOrEqual(ulong arg1, ulong arg2)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void LessOrEqual(decimal arg1, decimal arg2, string message, params object[] args)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void LessOrEqual(decimal arg1, decimal arg2, string message)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        public static void LessOrEqual(decimal arg1, decimal arg2)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void LessOrEqual(double arg1, double arg2, string message, params object[] args)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void LessOrEqual(double arg1, double arg2, string message)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        public static void LessOrEqual(double arg1, double arg2)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void LessOrEqual(float arg1, float arg2, string message, params object[] args)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void LessOrEqual(float arg1, float arg2, string message)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        public static void LessOrEqual(float arg1, float arg2)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,null, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void LessOrEqual(IComparable arg1, IComparable arg2, string message, params object[] args)
        {
           NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, args);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void LessOrEqual(IComparable arg1, IComparable arg2, string message)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,message, null);
        }
        
        /// <summary>
        /// Verifies that the first value is less than or equal to the second
        /// value. If it is not, then an
        /// <see cref="AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">The first value, expected to be less</param>
        /// <param name="arg2">The second value, expected to be greater</param>
        public static void LessOrEqual(IComparable arg1, IComparable arg2)
        {
            NewAssert.That(arg1, Is.LessThanOrEqualTo(arg2) ,null, null);
        }
        
        #endregion
        
        #region Contains
        
        /// <summary>
        /// Asserts that an object is contained in a list.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The list to be examined</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Contains(object expected, ICollection actual, string message, params object[] args)
        {
            NewAssert.That(actual, new CollectionContainsConstraint(expected) ,message, args);
        }
        
        /// <summary>
        /// Asserts that an object is contained in a list.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The list to be examined</param>
        /// <param name="message">The message to display in case of failure</param>
        public static void Contains(object expected, ICollection actual, string message)
        {
            NewAssert.That(actual, new CollectionContainsConstraint(expected) ,message, null);
        }
        
        /// <summary>
        /// Asserts that an object is contained in a list.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The list to be examined</param>
        public static void Contains(object expected, ICollection actual)
        {
            NewAssert.That(actual, new CollectionContainsConstraint(expected) ,null, null);
        }
        
        #endregion
        
    }
}
