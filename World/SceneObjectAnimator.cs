using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace World
{
    //Used to describe the transition of an scene object
    //between an set number of frames
    public class SceneObjectAnimator
    {
        private SceneAnimation _currentAnimation;
        private SceneAnimationKeyFrame _currentAnimationKeyFrame;
        private uint _currentFrame;
        private int _cycles;
        private bool _keyAdvancementForward;
        private int _keyFrame;
        private Vector _posDelta;
        public Queue<string> AnimationQueue;
        private readonly Dictionary<string, SceneAnimation> Animations;
        public bool completed;
        public SceneObject SceneObject;


        public SceneObjectAnimator(SceneObject obj)
        {
            _keyFrame = 0;
            _currentFrame = 0;
            _currentAnimationKeyFrame = null;
            completed = false;
            SceneObject = obj;
            _cycles = 0;
            Animations = new Dictionary<string, SceneAnimation>();
            AnimationQueue = new Queue<string>();
            State = AnimationState.Off;
            _keyAdvancementForward = true;
        }

        public AnimationState State { get; private set; }

        public IEnumerable<string> GetAnimationList()
        {
            return Animations.Keys.ToList();
        }

        public void AddAnimation(string name, SceneAnimation a)
        {
            //Dont Accept Empty Animations
            if (a.KeyFrames.Count == 0)
                return;

            if (a.KeyFrames.Count == 1)
                if (a.AnimationType == SceneAnimationType.FullRepeat || a.AnimationType == SceneAnimationType.SetRepeat)
                    return;

            if (!Animations.ContainsKey(name))
                Animations.Add(name, a);
            else
                Animations[name] = a;
        }

        private void EndAnimation()
        {
            _keyFrame = 0;
            _currentFrame = 0;
            _currentAnimationKeyFrame = null;
            _cycles = 0;
            AnimationQueue.Clear();
            _keyAdvancementForward = true;
            _currentAnimation = null;
        }

        public Vector DoFrameStep()
        {
            var Return = new Vector();

            if (_currentAnimation == null || SceneObject == null || State != AnimationState.Playing)
                return Return;

            CheckKeyState();

            if (completed)
            {
                EndAnimation();
                return Return;
            }


            if (_currentAnimationKeyFrame.FrameType == KeyFrameType.Movement)
                Return = _posDelta;

            _currentFrame++;

            return Return;
        }

        public void Play(string v)
        {
            if (!Animations.ContainsKey(v)) return;
            if (State == AnimationState.Playing || State == AnimationState.Paused)
                ResetState();
            _currentAnimation = Animations[v];

            //Dangerous, List could be empty
            NewKey(_currentAnimation.KeyFrames[0]);

            State = AnimationState.Playing;
        }

        public void Stop()
        {
            ResetState();
        }

        public void Pause()
        {
            State = AnimationState.Paused;
        }

        public void Resume()
        {
            State = AnimationState.Playing;
        }

        private void ResetState()
        {
            _keyFrame = 0;
            _currentFrame = 0;
            _currentAnimationKeyFrame = null;
            completed = false;
            _cycles = 0;
            AnimationQueue.Clear();
            _keyAdvancementForward = true;
        }

        private void CheckKeyState()
        {
            //Have we reached the end of a key frame
            if (_currentFrame <= _currentAnimationKeyFrame.TotalFrames) return;

            var end = false;
            if (_keyAdvancementForward)
            {
                if (_keyFrame == _currentAnimation.KeyFrames.Count - 1)
                    end = true;
            }
            else
            {
                if (_keyFrame == 0)
                    end = true;
            }

            if (end)
            {
                if (_currentAnimation.AnimationType == SceneAnimationType.OneShot)
                    completed = true;

                if (_currentAnimation.AnimationType == SceneAnimationType.SetRepeat)
                {
                    _cycles++;
                    if (_cycles >= _currentAnimation.cycles)
                        completed = true;
                }

                if (!completed)
                    switch (_currentAnimation.RepeatStyle)
                    {
                        case RepeatProgressionStyle.Circular:
                            _keyFrame = _keyAdvancementForward ? 0 : _currentAnimation.KeyFrames.Count - 1;
                            break;
                        case RepeatProgressionStyle.Reverse:
                            _keyFrame = _keyAdvancementForward ? --_keyFrame : ++_keyFrame;
                            _keyAdvancementForward = !_keyAdvancementForward;
                            break;
                        case RepeatProgressionStyle.None:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
            }
            else
            {
                _keyFrame = _keyAdvancementForward ? ++_keyFrame : --_keyFrame;
            }

            _currentFrame = 0;
            NewKey(_currentAnimation.KeyFrames[_keyFrame]);
        }

        private void NewKey(SceneAnimationKeyFrame key)
        {
            _currentAnimationKeyFrame = key;
            if (_currentAnimationKeyFrame.FrameType == KeyFrameType.Movement)
                calcDelta();
        }

        private void calcDelta()
        {
            var pos = _currentAnimationKeyFrame.TargetPos - SceneObject.Position;

            _posDelta = pos / _currentAnimationKeyFrame.TotalFrames;
        }
    }

    public class SceneAnimation
    {
        public SceneAnimationType AnimationType;
        public int cycles;

        public List<SceneAnimationKeyFrame> KeyFrames;
        public string Name;
        public RepeatProgressionStyle RepeatStyle;

        public SceneAnimation()
        {
            KeyFrames = new List<SceneAnimationKeyFrame>();
        }
    }

    //Describes an object state that an animation transitions towards and/or away from
    public class SceneAnimationKeyFrame
    {
        public KeyFrameType FrameType;
        public SceneObject KeyObject;
        public Vector TargetPos;
        public uint TotalFrames;

        public SceneAnimationKeyFrame(SceneObject obj)
        {
            KeyObject = obj;
            FrameType = KeyFrameType.Movement;
            TotalFrames = 0;
        }
    }

    [Flags]
    public enum KeyFrameType
    {
        Movement = 1
    }

    public enum SceneAnimationType
    {
        OneShot,
        SetRepeat,
        FullRepeat
    }

    public enum RepeatProgressionStyle
    {
        None,
        Circular,
        Reverse
    }

    public enum AnimationState
    {
        Off,
        Paused,
        Playing
    }
}