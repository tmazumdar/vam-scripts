using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SphereInteraction
{
    public class KeyMotion : MVRScript
    {
        protected List<Rigidbody> rigidBodies;
        private bool jumpPressed;
        private bool leftPressed;
        private bool rightPressed;
        private bool upPressed;
        private bool downPressed;

        protected JSONStorableBool jumpChoiceJSON;
        protected JSONStorableBool leftChoiceJSON;
        protected JSONStorableBool rightChoiceJSON;
        protected JSONStorableBool upChoiceJSON;
        protected JSONStorableBool downChoiceJSON;
        protected JSONStorableStringChooser receiverChoiceJSON;
        protected JSONStorableFloat forceFactorJSON;
        protected List<string> receiverChoices;
        protected Dictionary<string, ForceReceiver> receiverNameToForceReceiver;
        private JSONStorableBool translateChoiceJSON;

        protected void UpdateReceiver(string receiver)
        {
            if (receiver != null)
            {
                ForceReceiver fr;
                rigidBodies = new List<Rigidbody>();
                if (receiver == "All")
                {
                    rigidBodies = containingAtom.forceReceivers.Select(x => x.GetComponent<Rigidbody>()).ToList();
                }
                else if (receiverNameToForceReceiver.TryGetValue(receiver, out fr))
                {
                    rigidBodies.Add(fr.GetComponent<Rigidbody>());
                }
            }
        }

        public override void Init()
        {
            try
            {
                SuperController.LogMessage("Sphere Plugin Loaded");

                receiverChoices = new List<string>();
                receiverNameToForceReceiver = new Dictionary<string, ForceReceiver>();
                foreach (ForceReceiver fr in containingAtom.forceReceivers)
                {
                    receiverChoices.Add(fr.name);
                    receiverNameToForceReceiver.Add(fr.name, fr);
                }
                receiverChoices.Add("All");
                UpdateReceiver(containingAtom.forceReceivers.Select(x=>x.name).FirstOrDefault());

                receiverChoiceJSON = new JSONStorableStringChooser("receiver", receiverChoices, receiverChoices[0], "Receiver", UpdateReceiver);
                receiverChoiceJSON.storeType = JSONStorableParam.StoreType.Full;
                RegisterStringChooser(receiverChoiceJSON);
                UIDynamicPopup dp = CreateFilterablePopup(receiverChoiceJSON);
                dp.popupPanelHeight = 1000f;

                jumpChoiceJSON = new JSONStorableBool("Jump [Spacebar]", true);
                jumpChoiceJSON.storeType = JSONStorableParam.StoreType.Full;
                CreateToggle(jumpChoiceJSON);

                upChoiceJSON = new JSONStorableBool("Forward [Up]", true);
                upChoiceJSON.storeType = JSONStorableParam.StoreType.Full;
                CreateToggle(upChoiceJSON);

                downChoiceJSON = new JSONStorableBool("Back [Down]", true);
                downChoiceJSON.storeType = JSONStorableParam.StoreType.Full;
                CreateToggle(downChoiceJSON);
                                
                leftChoiceJSON = new JSONStorableBool("Left [Left]", true);
                leftChoiceJSON.storeType = JSONStorableParam.StoreType.Full;
                CreateToggle(leftChoiceJSON);

                rightChoiceJSON = new JSONStorableBool("Right [Up]", true);
                rightChoiceJSON.storeType = JSONStorableParam.StoreType.Full;
                CreateToggle(rightChoiceJSON);

                forceFactorJSON = new JSONStorableFloat("forceFactor", 10f, -50f, 50f, false, true);
                forceFactorJSON.storeType = JSONStorableParam.StoreType.Full;
                RegisterFloat(forceFactorJSON);
                CreateSlider(forceFactorJSON, true);

                translateChoiceJSON = new JSONStorableBool("Translate", false);
                CreateToggle(translateChoiceJSON, true);
            }
            catch (Exception e)
            {
                SuperController.LogError("Exception caught: " + e);
            }
        }

        // Start is called once before Update or FixedUpdate is called and after Init()
        void Start()
        {
            try
            {
                // put code in here
            }
            catch (Exception e)
            {
                SuperController.LogError("Exception caught: " + e);
            }
        }

        // Update is called with each rendered frame by Unity
        void Update()
        {
            try
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    jumpPressed = true;
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    rightPressed = true;
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    leftPressed = true;
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    upPressed = true;
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    downPressed = true;
                }
            }
            catch (Exception e)
            {
                SuperController.LogError("Exception caught: " + e);
            }
        }

        private Vector3 ComputeForce(Rigidbody RB, ForceProducerV2.AxisName axis)
        {
            Vector3 result;
            switch (axis)
            {
                case ForceProducerV2.AxisName.X:
                    result = RB.transform.right;
                    break;
                case ForceProducerV2.AxisName.NegX:
                    result = -RB.transform.right;
                    break;
                case ForceProducerV2.AxisName.Y:
                    result = RB.transform.up;
                    break;
                case ForceProducerV2.AxisName.NegY:
                    result = -RB.transform.up;
                    break;
                case ForceProducerV2.AxisName.Z:
                    result = RB.transform.forward;
                    break;
                case ForceProducerV2.AxisName.NegZ:
                    result = -RB.transform.forward;
                    break;
                default:
                    result = Vector3.zero;
                    break;
            }
            return result;
        }

        private void ApplyForces(List<Rigidbody> rigidBodies, ForceProducerV2.AxisName axis)
        {
            foreach (Rigidbody RB in rigidBodies)
            {
                Vector3 result = ComputeForce(RB, axis);
                if (translateChoiceJSON.val)
                {
                    RB.transform.Translate(result * 0.1f, Space.Self);
                }
                else 
                {
                    RB.AddForce(result * 100 * forceFactorJSON.val, ForceMode.Force);
                }                
            }
        }

        // FixedUpdate is called with each physics simulation frame by Unity
        void FixedUpdate()
        {
            try
            {
                if (jumpPressed)
                {
                    ApplyForces(rigidBodies, ForceProducerV2.AxisName.Y);
                    jumpPressed = false;
                }

                if (rightPressed)
                {
                    ApplyForces(rigidBodies, ForceProducerV2.AxisName.X);
                    rightPressed = false;
                }

                if (leftPressed)
                {
                    ApplyForces(rigidBodies, ForceProducerV2.AxisName.NegX);
                    leftPressed = false;
                }

                if (upPressed)
                {
                    ApplyForces(rigidBodies, ForceProducerV2.AxisName.Z);
                    upPressed = false;
                }

                if (downPressed)
                {
                    ApplyForces(rigidBodies, ForceProducerV2.AxisName.NegZ);
                    downPressed = false;
                }
            }
            catch (Exception e)
            {
                SuperController.LogError("Exception caught: " + e);
            }
        }
 
    }
}
