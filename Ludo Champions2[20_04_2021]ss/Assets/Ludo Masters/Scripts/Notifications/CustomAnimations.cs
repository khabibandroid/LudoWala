/*
 * Crated by Akrosh Chaudhary
 *  
 * This script is created for general animation and fucnction delay purpose. Feel free to modify as per use.
 */

using UnityEngine;
using System.Collections;

namespace UIUtilities
{
    public class CustomAnimations : MonoBehaviour
    {
        static CustomAnimations _instance;
        public static CustomAnimations instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = InstantiateNewObject();
                }
                else if (!_instance.gameObject.activeSelf)
                {
                    _instance = InstantiateNewObject();
                }
                return _instance;
            }
        }
        static CustomAnimations InstantiateNewObject()
        {
            GameObject go = new GameObject();
            go.AddComponent<CustomAnimations>();
            go.name = "CustomAnimationObject";
            return go.GetComponent<CustomAnimations>();
        }
    }

    public class Delay
    {
        #region Static Methods
        public static Action Function(System.Action function, float delay)
        {
            return new Action(function, delay);
        }

        public static Action<T> Function<T>(System.Action<T> function, T param, float delay)
        {
            return new Action<T>(function, param, delay);
        }

        public static Action<T1, T2> Function<T1, T2>(System.Action<T1, T2> function, T1 param1, T2 param2, float delay)
        {
            return new Action<T1, T2>(function, param1, param2, delay);
        }

        public static Action<T1, T2, T3> Function<T1, T2, T3>(System.Action<T1, T2, T3> function, T1 param1, T2 param2, T3 param3, float delay)
        {
            return new Action<T1, T2, T3>(function, param1, param2, param3, delay);
        }

        public static Action<T1, T2, T3, T4> Function<T1, T2, T3, T4>(System.Action<T1, T2, T3, T4> function, T1 param1, T2 param2, T3 param3, T4 param4, float delay)
        {
            return new Action<T1, T2, T3, T4>(function, param1, param2, param3, param4, delay);
        }
        #endregion

        #region Class Defination
        public class Action
        {
            Coroutine cr = null;
            System.Action function = null;
            float delay;

            public Action(System.Action _function, float _delay)
            {
                function = _function;
                delay = _delay;
                Invoke();
            }

            public void Invoke()
            {
                cr = CustomAnimations.instance.StartCoroutine(Cr_Function());
            }

            public void CancelAction()
            {
                if (cr != null) CustomAnimations.instance.StopCoroutine(cr);
            }

            IEnumerator Cr_Function()
            {
                if (delay > 0) yield return new WaitForSeconds(delay);
                function();
                cr = null;
            }
        }

        public class Action<T>
        {
            Coroutine cr = null;
            System.Action<T> function = null;
            T param;
            float delay;

            public Action(System.Action<T> _function, T _param, float _delay)
            {
                function = _function;
                param = _param;
                delay = _delay;
                Invoke();
            }

            public void Invoke()
            {
                cr = CustomAnimations.instance.StartCoroutine(Cr_Function());
            }

            public void CancelAction()
            {
                if (cr != null)
                    CustomAnimations.instance.StopCoroutine(cr);
            }

            IEnumerator Cr_Function()
            {
                if (delay > 0)
                    yield return new WaitForSeconds(delay);
                function(param);
                cr = null;
            }
        }

        public class Action<T1, T2>
        {
            Coroutine cr = null;
            T1 param1;
            T2 param2;
            System.Action<T1, T2> function = null;
            float delay;

            public Action(System.Action<T1, T2> _function, T1 _param1, T2 _param2, float _delay)
            {
                function = _function;
                param1 = _param1;
                param2 = _param2;
                delay = _delay;
                Invoke();
            }

            public void Invoke()
            {
                cr = CustomAnimations.instance.StartCoroutine(Cr_Function());
            }

            public void CancelAction()
            {
                if (cr != null)
                    CustomAnimations.instance.StopCoroutine(cr);
            }

            IEnumerator Cr_Function()
            {
                if (delay > 0)
                    yield return new WaitForSeconds(delay); ;
                function(param1, param2);
                cr = null;
            }
        }

        public class Action<T1, T2, T3>
        {
            Coroutine cr = null;
            System.Action<T1, T2, T3> funciton = null;
            T1 param1;
            T2 param2;
            T3 param3;
            float delay;

            public Action(System.Action<T1, T2, T3> _funciton, T1 _param1, T2 _param2, T3 _Param3, float _delay)
            {
                funciton = _funciton;
                param1 = _param1;
                param2 = _param2;
                param3 = _Param3;
                delay = _delay;
                Invoke();
            }

            public void Invoke()
            {
                cr = CustomAnimations.instance.StartCoroutine(Cr_Function());
            }

            public void CancelAction()
            {
                if (cr != null)
                    CustomAnimations.instance.StopCoroutine(cr);
            }

            IEnumerator Cr_Function()
            {
                if (delay > 0)
                    yield return new WaitForSeconds(delay);
                funciton(param1, param2, param3);
                cr = null;
            }
        }

        public class Action<T1, T2, T3, T4>
        {
            Coroutine cr = null;
            System.Action<T1, T2, T3, T4> funciton = null;
            T1 param1;
            T2 param2;
            T3 param3;
            T4 param4;
            float delay;

            public Action(System.Action<T1, T2, T3, T4> _funciton, T1 _param1, T2 _param2, T3 _Param3, T4 _param4, float _delay)
            {
                funciton = _funciton;
                param1 = _param1;
                param2 = _param2;
                param3 = _Param3;
                param4 = _param4;
                delay = _delay;
                Invoke();
            }

            public void Invoke()
            {
                cr = CustomAnimations.instance.StartCoroutine(Cr_Function());
            }

            public void CancelAction()
            {
                if (cr != null)
                    CustomAnimations.instance.StopCoroutine(cr);
            }

            IEnumerator Cr_Function()
            {
                if (delay > 0)
                    yield return new WaitForSeconds(delay);
                funciton(param1, param2, param3, param4);
                cr = null;
            }
        }
        #endregion
    }

    public class Interpolate
    {


        /// <summary>
        /// It scales the given object.
        /// </summary>
        /// <param name="transform">Transform of the object to scale</param>
        /// <param name="from">Starting position</param>
        /// <param name="to">Ending position</param> 
        /// <param name="duration">Time intervel to reach end position</param>
        public class Scale
        {

            public Coroutine cr = null;
            public AnimationCurve animCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });

            public bool isRunning
            {
                get
                {
                    return cr != null;
                }
            }
            public Scale(Transform _transform, Vector3 _from, Vector3 _to, float _duration)
            {
                cr = CustomAnimations.instance.StartCoroutine(Interpolation(_transform, _from, _to, _duration));
            }

            public void StopScaleAnimation()
            {
                if (cr != null)
                    CustomAnimations.instance.StopCoroutine(cr);
            }

            IEnumerator Interpolation(Transform transform, Vector3 from, Vector3 to, float duration)
            {
                float time = 0;
                while (time < duration)
                {
                    time += Time.deltaTime;
                    transform.localScale = Vector3.LerpUnclamped(from, to, animCurve.Evaluate(time / duration));
                    yield return new WaitForEndOfFrame();
                }
                transform.localScale = to;
                cr = null;
            }
        }


        /// <summary>
        /// Translates the given object.
        /// </summary>
        /// <param name="transform">Transform of the object to move</param>
        /// <param name="from">Starting position</param>
        /// <param name="to">Ending position</param> 
        /// <param name="duration">Time intervel to reach end position</param>
        /// <param name="isLocal">Decides wheather it local postion or global position</param>
        public class Position
        {

            public Coroutine cr = null;
            public AnimationCurve animCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });

            public bool isRunning
            {
                get
                {
                    return cr != null;
                }
            }
            public Position(Transform _transform, Vector3 _from, Vector3 _to, float _duration, bool _isLocal = true)
            {
                cr = CustomAnimations.instance.StartCoroutine(Interpolation(_transform, _from, _to, _duration, _isLocal));
            }

            public void StopPositionAnimation()
            {
                if (cr != null)
                    CustomAnimations.instance.StopCoroutine(cr);
            }

            IEnumerator Interpolation(Transform transform, Vector3 from, Vector3 to, float duration, bool local)
            {
                float time = 0;
                while (time < duration)
                {
                    time += Time.deltaTime;
                    if (local)
                        transform.localPosition = Vector3.Lerp(from, to, animCurve.Evaluate(time / duration));
                    else
                        transform.position = Vector3.Lerp(from, to, animCurve.Evaluate(time / duration));
                    yield return new WaitForEndOfFrame();
                }
                if (local)
                    transform.localPosition = to;
                else
                    transform.position = to;
                cr = null;
            }
        }

        /// <summary>
        /// Translates the given object.
        /// </summary>
        /// <param name="transform">Transform of the object to move</param>
        /// <param name="from">Starting position</param>
        /// <param name="to">Ending position</param> 
        /// <param name="duration">Time intervel to reach end position</param>
        /// <param name="isLocal">Decides wheather it local postion or global position</param>
        public class EulerAngles
        {

            public Coroutine cr = null;
            public AnimationCurve animCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });

            public bool isRunning
            {
                get
                {
                    return cr != null;
                }
            }
            public EulerAngles(Transform _transform, Vector3 _from, Vector3 _to, float _duration, bool _isLocal = true)
            {
                cr = CustomAnimations.instance.StartCoroutine(Interpolation(_transform, _from, _to, _duration, _isLocal));
            }

            public void StopEulerAnglesAnimation()
            {
                if (cr != null)
                    CustomAnimations.instance.StopCoroutine(cr);
            }

            IEnumerator Interpolation(Transform transform, Vector3 from, Vector3 to, float duration, bool local)
            {
                float time = 0;
                while (time < duration)
                {
                    time += Time.deltaTime;
                    if (local)
                        transform.localEulerAngles = Vector3.Lerp(from, to, animCurve.Evaluate(time / duration));
                    else
                        transform.eulerAngles = Vector3.Lerp(from, to, animCurve.Evaluate(time / duration));
                    yield return new WaitForEndOfFrame();
                }
                if (local)
                    transform.localEulerAngles = to;
                else
                    transform.eulerAngles = to;
                cr = null;
            }
        }
    }
}