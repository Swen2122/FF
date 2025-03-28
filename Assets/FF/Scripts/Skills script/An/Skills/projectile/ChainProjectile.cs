using UnityEngine;
using DG.Tweening;
using System.Linq;

public class ChainProjectile : DefaultProjectile
{
    [Header("Chain Settings")]
    [SerializeField] private float chainRange = 5f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Element damageType;
    [SerializeField] private int maxJumps = 3;
    [SerializeField] private bool randomTargetSelection = false;
    [SerializeField] private float jumpDuration = 0.3f;
    [SerializeField] private float jumpDelay = 0.3f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private Ease jumpEase = Ease.OutQuad;

    [Header("Visual Settings")]
    [SerializeField] private ParticleSystem jumpEffect;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private bool rotateTowardsTarget = true;
    [SerializeField] private float rotationSpeed = 720f;

    [Header("Prediction Settings")]
    [SerializeField] private bool usePredictiveTargeting = true;
    [SerializeField] private float maxPredictionTime = 0.5f; // ������������ ��� ������������
    [SerializeField] private int predictionIterations = 3; // ʳ������ �������� ��� ��������� ������������
    
    private GameObject lastTarget;
    private int currentJumps = 0;
    private Sequence currentJumpSequence;
    private Vector3 lastValidPosition;
    // ��������� ���������� ��� ����������
    private struct TargetInfo
    {
        public Collider2D Collider;
        public Rigidbody2D Rigidbody;
        public Vector2 Position;
        public Vector2 Velocity;
        public float Distance;
    }
    protected override void OnHit(Collider2D other)
    {
        if (!IsValidTarget(other)) return;

        if (other.TryGetComponent<ICanHit>(out var target))
        {
            // ��������� ������� �������� ������
            if (currentJumpSequence != null && currentJumpSequence.IsPlaying())
            {
                Vector3 hitPoint = other.ClosestPoint(transform.position);

                // ��������� ������� �������� ���������� ��������� ����
                currentJumpSequence.Kill();
                transform.DOMove(hitPoint, 0.1f)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => {
                        target.TakeHit(damage, damageType);
                        lastTarget = other.gameObject;

                        if (currentJumps < maxJumps)
                        {
                            JumpToNextTarget();
                        }
                        else
                        {
                            StopAndDestroy();
                        }
                    });
            }
            else
            {
                // ���� �������� ����, ���������� ��������� ������
                target.TakeHit(damage, damageType);
                lastTarget = other.gameObject;

                if (currentJumps < maxJumps)
                {
                    JumpToNextTarget();
                }
                else
                {
                    StopAndDestroy();
                }
            }
        }
    }
    

    private Vector2 PredictTargetPosition(TargetInfo target)
    {
        if (!usePredictiveTargeting || target.Rigidbody == null || target.Rigidbody.linearVelocity.sqrMagnitude < 0.1f)
            return target.Position;

        float timeToTarget = jumpDuration;
        Vector2 predictedPos = target.Position;

        // ���������� ��������� ���� ������������
        for (int i = 0; i < predictionIterations; i++)
        {
            // ����������� �� ���� ���� ����� timeToTarget ������
            Vector2 futurePos = target.Position + target.Velocity * timeToTarget;

            // ����������� ����� ���, ���� ������� ��� �������� �� ���� �������
            float distance = Vector2.Distance(transform.position, futurePos);
            float newTimeToTarget = distance / (chainRange / jumpDuration);

            // �������� ��� ������������
            timeToTarget = Mathf.Min(newTimeToTarget, maxPredictionTime);
            predictedPos = futurePos;
        }

        return predictedPos;
    }

    private TargetInfo[] GetNearbyTargets()
    {
        return Physics2D.OverlapCircleAll(transform.position, chainRange, targetLayer)
            .Where(t => t.gameObject != lastTarget)
            .Select(t => new TargetInfo
            {
                Collider = t,
                Rigidbody = t.GetComponent<Rigidbody2D>(),
                Position = t.transform.position,
                Velocity = t.GetComponent<Rigidbody2D>()?.linearVelocity ?? Vector2.zero,
                Distance = Vector2.Distance(transform.position, t.transform.position)
            })
            .OrderBy(t => t.Distance)
            .ToArray();
    }
    private void JumpToNextTarget()
    {
        var nearbyTargets = GetNearbyTargets();
        if (nearbyTargets.Length == 0)
        {
            StopAndDestroy();
            return;
        }

        // ���� ��� (�������� �� ���������)
        var target = randomTargetSelection
            ? nearbyTargets[Random.Range(0, nearbyTargets.Length)]
            : nearbyTargets[0];

        // ����������� �������
        Vector3 predictedPosition = PredictTargetPosition(target);

        // ����� �������� (���� �������)
        if (jumpDelay > 0)
        {
            DOTween.Sequence()
                .AppendInterval(jumpDelay)
                .AppendCallback(() => PerformJump(transform.position, predictedPosition, target));
        }
        else
        {
            PerformJump(transform.position, predictedPosition, target);
        }

        currentJumps++;
    }

    private void PerformJump(Vector3 startPos, Vector3 targetPos, TargetInfo target)
    {
        currentJumpSequence?.Kill();

        // ��������� ������������ ��� �������
        currentJumpSequence = DOTween.Sequence();

        currentJumpSequence.Append(DOTween.To(
            () => 0f,
            (float progress) =>
            {
            // �������� ��������� ������� ������� (�� ��������)
            if (usePredictiveTargeting && progress < 0.8f)
                {
                    var updatedTarget = new TargetInfo
                    {
                        Position = target.Collider.transform.position,
                        Velocity = target.Rigidbody?.linearVelocity ?? Vector2.zero
                    };
                    targetPos = PredictTargetPosition(updatedTarget);
                }

            // ����� ���� �� �������
            float x = Mathf.Lerp(startPos.x, targetPos.x, progress);
                float y = Mathf.Lerp(startPos.y, targetPos.y, progress) +
                         jumpHeight * Mathf.Sin(progress * Mathf.PI);

                transform.position = new Vector3(x, y, transform.position.z);

            // ��������� �� ���
            if (rotateTowardsTarget)
                {
                    Vector3 direction = (targetPos - transform.position).normalized;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        Quaternion.Euler(0, 0, angle),
                        rotationSpeed * Time.deltaTime
                    );
                }
            },
            1f,
            jumpDuration
        )).SetEase(jumpEase);

        currentJumpSequence
            .OnStart(() => PlayJumpEffects(targetPos)) // �������� ������
            .OnComplete(() => OnProjectileReachedTarget());

        lastTarget = target.Collider.gameObject;
    }
    private bool IsValidTarget(Collider2D other)
    {
        return other.gameObject.layer == targetLayer &&
               other.gameObject != lastTarget;
    }

    private void PlayJumpEffects(Vector3 position)
    {
        if (jumpEffect != null)
        {
            var effect = Instantiate(jumpEffect, position, Quaternion.identity);
            effect.transform.parent = null;
            Destroy(effect.gameObject, effect.main.duration);
        }

        if (jumpSound != null)
        {
            AudioSource.PlayClipAtPoint(jumpSound, position);
        }
    }
  
    protected override void OnProjectileReachedTarget()
    {
        if (currentJumps >= maxJumps)
        {
            StopAndDestroy();
            return;
        }
        JumpToNextTarget();
    }

    private void StopAndDestroy()
    {
        currentJumpSequence?.Kill();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        currentJumpSequence?.Kill();
    }

    public override void Initialize(ProjectileData projectileData, Vector2 target, Element element)
    {
        base.Initialize(projectileData, target, element);
        currentElement = element;
        lastValidPosition = transform.position;
    }
}