using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class EnemyZombie : MonoBehaviour
{
    [Header("Detection")]
    public float chaseDistance = 8f;
    public float runSpeed = 3.5f;

    [Header("Audio")]
    public AudioClip deathSound;

    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");
    private static readonly int Death1Hash = Animator.StringToHash("Death1");
    private static readonly int Death2Hash = Animator.StringToHash("Death2");

    private Animator _animator;
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
    private Transform _player;
    private bool _hasDetectedPlayer;
    private bool _isDead;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        FindPlayer();
    }

    private void Update()
    {
        if (_isDead)
        {
            return;
        }

        if (_player == null)
        {
            FindPlayer();
            if (_player == null)
            {
                return;
            }
        }

        float distance = Vector3.Distance(transform.position, _player.position);
        if (!_hasDetectedPlayer && distance <= chaseDistance)
        {
            _hasDetectedPlayer = true;
            SetRunning(true);
        }

        if (_hasDetectedPlayer)
        {
            MoveTowardsPlayer();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.1f);
        Gizmos.DrawSphere(transform.position, chaseDistance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isDead)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerDeath playerDeath = collision.gameObject.GetComponentInParent<PlayerDeath>();
            if (playerDeath != null)
            {
                playerDeath.KillPlayer();
            }
            return;
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            _isDead = true;
            SetRunning(false);
            SetWalking(false);
            SetAttacking(false);

            if (_audioSource != null && deathSound != null)
            {
                _audioSource.PlayOneShot(deathSound);
            }

            if (Random.value < 0.5f)
            {
                PlayDeath1();
            }
            else
            {
                PlayDeath2();
            }

            DisableColliders();
        }
    }

    public void SetWalking(bool isWalking)
    {
        _animator.SetBool(IsWalkingHash, isWalking);
        if (isWalking)
        {
            _animator.SetBool(IsRunningHash, false);
            _animator.SetBool(IsAttackingHash, false);
        }
    }

    public void SetRunning(bool isRunning)
    {
        _animator.SetBool(IsRunningHash, isRunning);
        if (isRunning)
        {
            _animator.SetBool(IsWalkingHash, false);
            _animator.SetBool(IsAttackingHash, false);
        }
    }

    public void SetAttacking(bool isAttacking)
    {
        _animator.SetBool(IsAttackingHash, isAttacking);
        if (isAttacking)
        {
            _animator.SetBool(IsWalkingHash, false);
            _animator.SetBool(IsRunningHash, false);
        }
    }

    public void PlayDeath1()
    {
        _animator.SetTrigger(Death1Hash);
    }

    public void PlayDeath2()
    {
        _animator.SetTrigger(Death2Hash);
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        _player = playerObject != null ? playerObject.transform : null;
    }

    private void MoveTowardsPlayer()
    {
        Vector3 targetPosition = new Vector3(_player.position.x, transform.position.y, _player.position.z);
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, runSpeed * Time.deltaTime);

        if (_rigidbody != null && !_rigidbody.isKinematic)
        {
            _rigidbody.MovePosition(newPosition);
        }
        else
        {
            transform.position = newPosition;
        }

        Vector3 direction = targetPosition - transform.position;
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 8f * Time.deltaTime);
        }
    }

    private void DisableColliders()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic = true;
        }
    }
}
