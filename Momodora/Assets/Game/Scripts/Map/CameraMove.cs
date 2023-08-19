using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Vector2Int fieldSize = Vector2Int.one;
    Vector2 mapSize = new Vector2(7 * Screen.width / Screen.height, 7);
    PlayerMove player;

    Coroutine coroutine;
    Vector3 shaking;

    float camHeight;
    float camWidth;

    public void CameraOnceMove(int fieldIndex, int type)
    {
        if (fieldIndex == 1)
        {
            transform.position = new Vector3(0, 0, -10) + shaking;
        }
        else if (fieldIndex >1 && type==1)
        {
            transform.position = new Vector3((fieldSize.x - 1) * camWidth * 2 + (fieldSize.x - 1) * (13 - camWidth) * 2, transform.position.y, -10) + shaking;
        }
        else if (fieldIndex > 1 && type == 2)
        {
            transform.position = new Vector3(transform.position.x, -(camHeight * (fieldSize.y - 1) * 2), -10) + shaking;
        }
    }

    public static void ShakingCamera(CameraMove camera)
    {
        if (camera.coroutine != null)
        {
            camera.StopCoroutine(camera.coroutine);
        }

        camera.coroutine = camera.StartCoroutine(camera.ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        for (int i = 0; i < 8; i++)
        {
            shaking = Random.insideUnitCircle;
            yield return new WaitForEndOfFrame();
            shaking = Vector3.zero;
        }
        shaking = Vector3.zero;
    }

    private void Start()
    {
        shaking = Vector3.zero;
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Screen.width / Screen.height;
    }

    private void Update()
    {
        if (GameManager.instance.checkMapUpdate)
        {
            if (GameManager.instance.LoadSuccess())
            {
                fieldSize = GameManager.instance.currMap.fieldSize;
                GameManager.instance.checkMapUpdate = false;
            }
        }

        if (player == null)
        {
            player = FindObjectOfType<PlayerMove>();
        }

        if (GameManager.instance.cameraStop)
        {
            return;
        }


        if (player.transform.position.x > 0 && player.transform.position.x < (fieldSize.x - 1) * camWidth * 2 + (fieldSize.x - 1) * (13 - camWidth) * 2)
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, -10) + shaking;

        }
        else if (player.transform.position.x <= 0)
        {
            transform.position = new Vector3(0, transform.position.y, -10) + shaking;
        }
        else if (player.transform.position.x >= (fieldSize.x - 1) * camWidth * 2 + (fieldSize.x - 1) * (13 - camWidth) * 2)
        {
            transform.position = new Vector3((fieldSize.x - 1) * camWidth * 2 + (fieldSize.x - 1) * (13 - camWidth) * 2, transform.position.y, -10) + shaking;
        }


        if (player.transform.position.y < 0 && player.transform.position.y > -(camHeight * (fieldSize.y - 1) * 2))
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, -10) + shaking;

        }
        else if (player.transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, -10) + shaking;
        }
        else if (player.transform.position.y <= -(camHeight * (fieldSize.y - 1) * 2))
        {
            transform.position = new Vector3(transform.position.x, -(camHeight * (fieldSize.y - 1) * 2), -10) + shaking;
        }
    }
}
