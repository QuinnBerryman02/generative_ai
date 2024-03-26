using System.Linq;
using UnityEngine;
using Dummies;
using Mic = UnityEngine.Microphone;
using UnityEngine.XR;
using Valve.VR;
namespace Wrappers.Core {
    public class Microphone : UpdateWrapper<object, DataList> {
        private SteamVR_Action_Boolean sysMic = SteamVR_Actions.default_SysMIC;
        [SerializeField] Material micMaterial;
        [SerializeField] AudioSource playback;
        [SerializeField] string[] devices = {};
        [SerializeField] int selectedDevice = 0; 

        private AudioClip m_audio;

        public override DataList OnUpdate(object t) {
            devices = Mic.devices;
            if(KeyDown()) {
                m_audio = Mic.Start(Mic.devices[selectedDevice],false,20,44100);
            }
            if(KeyPressed()) {
                micMaterial.color = Color.green;
            } else {
                micMaterial.color = Color.red;
            }
            if(KeyUp()) {
                Mic.End(Mic.devices[selectedDevice]);
                // playback.PlayOneShot(m_audio);
                return new(content: new() { new Entry<Audio>("audio", new(m_audio)) });
            }
            throw new SkipExeception();
        }

        private bool KeyDown() {
            if(XRSettings.enabled) {
                return sysMic.GetStateDown(SteamVR_Input_Sources.Any);
            } else {
                return Input.GetKeyDown(KeyCode.Space);
            }
        }

        private bool KeyPressed() {
            if(XRSettings.enabled) {
                return sysMic.GetState(SteamVR_Input_Sources.Any);
            } else {
                return Input.GetKey(KeyCode.Space);
            }
        }

        private bool KeyUp() {
            if(XRSettings.enabled) {
                return sysMic.GetStateUp(SteamVR_Input_Sources.Any);
            } else {
                return Input.GetKeyUp(KeyCode.Space);
            }
        }
    }
}