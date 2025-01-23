using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInShield : MonoBehaviour
{
    [SerializeField] private float duration = 2f;
    [SerializeField] private float shieldSize = 3f;

    private float childShieldSize = 1.766f;

    public void ShowShield()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleSystem childPs = transform.GetChild(0).GetComponent<ParticleSystem>();
        var mainModule = ps.main;
        var childMainModule = childPs.main;

        mainModule.startSize = shieldSize;
        childMainModule.startSize = shieldSize * childShieldSize;

        Color startColor = mainModule.startColor.color;
        Color childStartColor = childMainModule.startColor.color;

        startColor.a = 0f;
        childStartColor.a = 0f;

        gameObject.SetActive(true);

        DOTween.To(() => mainModule.startColor.color.a,
            x => startColor.a = x,
            1f,
            duration).OnUpdate(() =>
        {
            mainModule.startColor = startColor;
        });

        DOTween.To(() => childMainModule.startColor.color.a,
            x => childStartColor.a = x,
            1f,
            duration).OnUpdate(() =>
            {
                childMainModule.startColor = childStartColor;
            });
    }

    public void HideShield()
    {
        gameObject.SetActive(false);
    }
}
