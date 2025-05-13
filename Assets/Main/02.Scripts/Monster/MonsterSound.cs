using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _dead;
    public void DeadSound()
    {
        _audio.clip = _dead;
        _audio.Play(); 
    }
}
