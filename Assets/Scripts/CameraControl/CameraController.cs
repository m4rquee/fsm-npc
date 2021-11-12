using UnityEngine;

namespace CameraControl
{
    public class CameraController : MonoBehaviour
    {
        public float panSpeed = 20f;
        public float xMin, xMax;
        public float zMin, zMax;

        public float scrollSpeed = 2000f;
        public float yMin, yMax;

        public float rotationSpeed = 20f;
        public float rotMin, rotMax;

        private void Update()
        {
            var pos = transform.position;
            if (Input.GetKey("w"))
                pos.z += panSpeed * Time.deltaTime;
            if (Input.GetKey("s"))
                pos.z -= panSpeed * Time.deltaTime;
            if (Input.GetKey("a"))
                pos.x -= panSpeed * Time.deltaTime;
            if (Input.GetKey("d"))
                pos.x += panSpeed * Time.deltaTime;

            var scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y += scroll * scrollSpeed * Time.deltaTime;

            pos.x = Mathf.Clamp(pos.x, xMin, xMax);
            pos.y = Mathf.Clamp(pos.y, yMin, yMax);
            pos.z = Mathf.Clamp(pos.z, zMin, zMax);
            pos.z = Mathf.Clamp(pos.z, zMin, zMax);
            transform.position = pos;

            var rot = transform.rotation.eulerAngles;
            if (Input.GetKey("q"))
                rot.y -= rotationSpeed * Time.deltaTime;
            if (Input.GetKey("e"))
                rot.y += rotationSpeed * Time.deltaTime;

            rot.y = Mathf.Clamp(rot.y, rotMin, rotMax);
            transform.rotation = Quaternion.Euler(rot);
        }
    }
}