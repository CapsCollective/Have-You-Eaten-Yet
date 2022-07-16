using System.Collections;
using UnityEngine;

// Credit YarnSpinner
// https://github.com/YarnSpinnerTool/ExampleProjects/

// A class that adds extension methods to MonoBehaviour that allows for
// animating a property over time.
public static class TweenExtensions
{

    // Defines a type of function that can be called over time, taking a start
    // and end value, and a 'factor' value between 0 and 1.
    public delegate void TweenFunction<T>(T from, T to, float factor);

    // Starts and returns a coroutine that begins running an animation from
    // 'from' to 'to' over 'time' seconds, calling the 'handler' function every
    // frame, and calling the optional 'onComplete' method when it's done.
    //
    // This method is an 'extension' method: because it's in a static class, is
    // static itself, and the first parameter has the 'this' keyword, C# will
    // act as though this 'Tween' method is a part of the existing
    // 'MonoBehaviour' class. In effect, we're adding a new method to the
    // Unity-provided class!
    //
    // Additionally, we're making this method use a type parameter 'T'. Because
    // the tweening system doesn't do anything with the 'from' and 'to' values
    // itself, it doesn't actually matter what type they are, so we'll leave
    // this open to interpretation. This means you could use this method to
    // tween between two values of any type - integers, floats, Vector3's, even
    // strings! It's up to the 'handler' method to decide what to do at every
    // frame.
    public static Coroutine Tween<T>(this MonoBehaviour behaviour, T from, T to, float time, TweenFunction<T> handler, System.Action onComplete = null)
    {

        // Coroutines run on specific MonoBehaviours, so we'll tell the
        // 'behaviour' to start running it our HandleTween coroutine.
        return behaviour.StartCoroutine(HandleTween(from, to, time, handler, onComplete));
    }

    // The coroutine that actually performs the work of animating the change.
    private static IEnumerator HandleTween<T>(T from, T to, float time, TweenFunction<T> handler, System.Action onComplete)
    {
        // How much time has elapsed since we started this change.
        var timeElapsed = 0f;

        // Loop until 'time' seconds have elapsed.
        while (timeElapsed < time)
        {

            // Our handler function expects the 'factor' parameter to be between
            // zero and one, where zero is the start of the animation and one is
            // the end of the animation.
            //
            // We'll get this value by dividing timeElapsed by total time, and
            // then clamping the result to between 0 and 1.
            float factor = Mathf.Clamp01(timeElapsed / time);

            // We have everything we need - call the handler function to perform
            // whatever specific work we need to do.
            handler(from, to, factor);

            // Update our elapsed time.
            timeElapsed += Time.deltaTime;

            // Yield the coroutine so that other game work can be done. (This is
            // very important! If you forget this, Unity will freeze up.)
            yield return null;
        }

        // We've reached the end of our animation. Tidy up by calling the
        // handler one last time, with a value of 1. (It's very unlikely that
        // the 'factor' variable we calcuate above would reach an even value of
        // 1.0, and certain animations would look strange if they end early,
        // like an object's transparency.) To deal with this, we'll call it one
        // last time with the final value.

        handler(from, to, 1f);

        // Finally, if we had an on-complete method to call, call it now.
        onComplete?.Invoke();
    }
}
