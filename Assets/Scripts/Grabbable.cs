using UnityEngine;
using UnityEngine.Animations;

public class Grabbable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject grabIndicatorPrefab;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite grabbedSprite;
    //[SerializeField] private Collider collider;

    private SpriteRenderer spriteRenderer;
    private GameObject grabIndicator;

    private void Awake()
    {
        gameObject.tag = "Grabbable";
        grabIndicator = Instantiate(grabIndicatorPrefab, transform.position, Quaternion.identity, transform);
        spriteRenderer = grabIndicator.GetComponent<SpriteRenderer>();
        grabIndicator.SetActive(false);
        ConstraintSource source = new ConstraintSource
        {
            sourceTransform = Camera.main.transform,
            weight = 1
        };
        grabIndicator.GetComponent<LookAtConstraint>().SetSource(0, source);
    }

    public void Select()
    {
        spriteRenderer.sprite = selectedSprite;
        grabIndicator.SetActive(true);
    }

    public void UnSelect()
    {
        grabIndicator.SetActive(false);
    }

    public void Grab()
    {
        spriteRenderer.sprite = grabbedSprite;
        grabIndicator.SetActive(true);
    }
}
