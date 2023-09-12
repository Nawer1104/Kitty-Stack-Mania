using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool _dragging = true;

    private Vector2 _offset;

    public static bool mouseButtonReleased;

    public GameObject vfxSuccess;
    public GameObject vfxFail;


    private void Awake()
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void OnMouseDown()
    {
        if (!_dragging) return;
        _offset = GetMousePos() - (Vector2)transform.position;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    private void OnMouseDrag()
    {
        if (!_dragging) return;

        var mousePosition = GetMousePos();

        transform.position = mousePosition - _offset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null )
        {
            if (collision.gameObject.tag == "Destroy")
            {
                SpawnCat.Instance.activeCat.Remove(gameObject);
                if (SpawnCat.Instance.catStack.Contains(this))
                {
                    SpawnCat.Instance.catStack.Remove(this);
                }
                GameObject vfx = Instantiate(vfxFail, transform.position, Quaternion.identity);
                Destroy(vfx, 1f);
                Destroy(gameObject);
            }

            if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Cat" && collision.gameObject.GetComponent<DragAndDrop>()._dragging == false)
            {
                SpawnCat.Instance.activeCat.Remove(gameObject);
                SpawnCat.Instance.Spawn();
                if (!SpawnCat.Instance.catStack.Contains(this))
                {
                    SpawnCat.Instance.catStack.Add(this);
                }
                SpawnCat.Instance.CheckWin();
            }
        }
    }


    private void OnMouseUp()
    {
        mouseButtonReleased = true;
        _dragging = false;
    }

    private Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private IEnumerator VFXCourutine()
    {
        GameObject vfx = Instantiate(vfxSuccess, transform.position, Quaternion.identity);
        Destroy(vfx, 1f);

        yield return new WaitForSeconds(1f);

        GameManager.Instance.levels[GameManager.Instance.GetCurrentIndex()].gameObjects.Remove(gameObject);
        GameManager.Instance.CheckLevelUp();
    }
}