using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public ParticleSystem laserStartParticles;
    public ParticleSystem laserEndParticles;
    public float lineLength = 10f;
    public LayerMask layerMask;

    private AudioSource audioSource;
    private LineRenderer line;
    private bool sfxIsPlaying = false;
    private bool startParticlesPlaying = false;
    private bool endParticlesPlaying = false;
    private RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (startParticlesPlaying == false)
            {
                startParticlesPlaying = true;
                laserStartParticles.Play(true);
            }
            laserStartParticles.gameObject.transform.position = transform.position;
            line.enabled = true;
            if (sfxIsPlaying == false)
            {
                sfxIsPlaying = true;
                audioSource.Play();
            }

            hit = Physics2D.Raycast(transform.position, Vector2.right, lineLength, layerMask);
            if (hit)
            {
                if(endParticlesPlaying == false)
                {
                    endParticlesPlaying = true;
                    laserEndParticles.Play(true);
                }
                laserEndParticles.gameObject.transform.position = hit.point;
                float distance = ((Vector2)hit.point - (Vector2)transform.position).magnitude;
                line.SetPosition(1, new Vector3(distance, 0, 0));
            }
            else
            {
                line.SetPosition(1, new Vector3(lineLength, 0, 0));
                endParticlesPlaying = false;
                laserEndParticles.Stop(true);

            }
        }

        else
        {
            line.SetPosition(1, new Vector3(lineLength, 0, 0));
            endParticlesPlaying = false;
            laserEndParticles.Stop(true);
            startParticlesPlaying = false;
            laserStartParticles.Stop(true);
            sfxIsPlaying = false;
            audioSource.Stop();
            line.enabled = false;


        }
    }
}
