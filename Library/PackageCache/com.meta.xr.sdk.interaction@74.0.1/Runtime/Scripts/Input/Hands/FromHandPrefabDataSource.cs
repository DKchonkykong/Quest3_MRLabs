/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;
using UnityEngine;

namespace Oculus.Interaction.Input
{
    public class FromHandPrefabDataSource : DataSource<HandDataAsset>
    {
        private readonly HandDataAsset _handDataAsset = new HandDataAsset();
        protected override HandDataAsset DataAsset => _handDataAsset;

        [SerializeField]
        private Handedness _handedness;
        public Handedness Handedness => _handedness;

        [SerializeField]
        private bool _hidePrefabOnStart = true;

        [HideInInspector]
        [SerializeField]
        private List<Transform> _jointTransforms = new List<Transform>();

        [HideInInspector]
        [SerializeField]
        private List<Transform> _jointTransformsOpenXR = new List<Transform>();

#if ISDK_OPENXR_HAND
        public List<Transform> JointTransforms => _jointTransformsOpenXR;
#else
        public List<Transform> JointTransforms => _jointTransforms;
#endif

        [SerializeField, Interface(typeof(IHandSkeletonProvider))]
        private UnityEngine.Object _handSkeletonProvider;
        private IHandSkeletonProvider HandSkeletonProvider;

        [SerializeField, Interface(typeof(ITrackingToWorldTransformer)), Optional]
        private UnityEngine.Object _trackingToWorldTransformer;
        private ITrackingToWorldTransformer TrackingToWorldTransformer;

        protected virtual void Awake()
        {
            HandSkeletonProvider = _handSkeletonProvider as IHandSkeletonProvider;
            if (_trackingToWorldTransformer != null)
            {
                TrackingToWorldTransformer =
                    _trackingToWorldTransformer as ITrackingToWorldTransformer;
            }
            HandDataSourceConfig Config = _handDataAsset.Config;
            Config.Handedness = _handedness;
        }

        protected override void Start()
        {
            base.Start();
            HandDataSourceConfig Config = _handDataAsset.Config;
            Config.TrackingToWorldTransformer = TrackingToWorldTransformer;
            Config.HandSkeleton = HandSkeletonProvider[_handedness];
            if (!_hidePrefabOnStart)
            {
                return;
            }

            foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
            {
                r.enabled = false;
            }
        }

        protected override void UpdateData()
        {
            this.AssertField(HandSkeletonProvider, nameof(HandSkeletonProvider));

            _handDataAsset.IsDataValid = true;
            _handDataAsset.IsConnected = true;
            _handDataAsset.IsTracked = true;
            _handDataAsset.IsHighConfidence = true;
            _handDataAsset.RootPoseOrigin = PoseOrigin.SyntheticPose;
            _handDataAsset.Root = this.transform.GetPose();
            _handDataAsset.HandScale = 1;

            for (var i = 0; i < Constants.NUM_HAND_JOINTS; ++i)
            {
                Transform joint = JointTransforms[i];

                if (joint == null)
                {
#pragma warning disable 0618
                    _handDataAsset.Joints[i] = Quaternion.identity;
#pragma warning restore 0618
#if ISDK_OPENXR_HAND
                    _handDataAsset.JointPoses[i] = new Pose(Vector3.zero, Quaternion.identity);
#endif
                    continue;
                }

#pragma warning disable 0618
                _handDataAsset.Joints[i] = joint.transform.localRotation;
#pragma warning restore 0618

#if ISDK_OPENXR_HAND
                Pose pose = joint.transform.GetPose(Space.World);
                pose = PoseUtils.Delta(transform, pose);
                _handDataAsset.JointPoses[i] = pose;
#endif
            }
        }

        public Transform GetTransformFor(HandJointId jointId)
        {
            return JointTransforms[(int)jointId];
        }
    }
}
