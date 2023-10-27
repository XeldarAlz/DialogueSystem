using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerBody;
    [SerializeField] private float _maxMovementSpeed;
    [SerializeField] private float _runSpeedMultiplier;

    private const byte IDLE_SPEED = 0;
    private const float DIAGONAL_SPEED_FACTOR = 0.7075f;

    private MovementInput _movementInput = new MovementInput();
    private float _diagonalSpeed;
    private float _movementSpeed;
    
    public MovementDirection MoveDirection { get; private set; } = MovementDirection.Idle;

    private void OnDisable()
    {
        StopMovement();
    }

    private void Update()
    {
        MoveDirection = _movementInput.GetCurrentInputs();
        MoveToDirection(MoveDirection);

        if (MoveDirection == MovementDirection.Idle) return;

        AdjustMovementSpeed();
    }

    private void AdjustMovementSpeed()
    {
        _movementSpeed = Mathf.Lerp(_movementSpeed, _maxMovementSpeed, Time.deltaTime);
        _movementSpeed = Input.GetKey(KeyCode.LeftShift) ? _movementSpeed * _runSpeedMultiplier : _movementSpeed;

        _movementSpeed = Mathf.Clamp(_movementSpeed, 0, _maxMovementSpeed * _runSpeedMultiplier);
        SetDiagonalMovementSpeed();
    }

    private void SetDiagonalMovementSpeed()
    {
        _diagonalSpeed = _movementSpeed * DIAGONAL_SPEED_FACTOR;
    }

    private void MoveToDirection(MovementDirection direction)
    {
        switch (direction)
        {
            case MovementDirection.Upward:
                _playerBody.velocity = new Vector3(IDLE_SPEED, _playerBody.velocity.y, _movementSpeed);
                break;
            case MovementDirection.Downward:
                _playerBody.velocity = new Vector3(IDLE_SPEED, _playerBody.velocity.y, -_movementSpeed);
                break;
            case MovementDirection.Left:
                _playerBody.velocity = new Vector3(-_movementSpeed, _playerBody.velocity.y, IDLE_SPEED);
                break;
            case MovementDirection.Right:
                _playerBody.velocity = new Vector3(_movementSpeed, _playerBody.velocity.y, IDLE_SPEED);
                break;
            case MovementDirection.RightUp:
                _playerBody.velocity = new Vector3(_diagonalSpeed, _playerBody.velocity.y, _diagonalSpeed);
                break;
            case MovementDirection.RightDown:
                _playerBody.velocity = new Vector3(_diagonalSpeed, _playerBody.velocity.y, -_diagonalSpeed);
                break;
            case MovementDirection.LeftUp:
                _playerBody.velocity = new Vector3(-_diagonalSpeed, _playerBody.velocity.y, _diagonalSpeed);
                break;
            case MovementDirection.LeftDown:
                _playerBody.velocity = new Vector3(-_diagonalSpeed, _playerBody.velocity.y, -_diagonalSpeed);
                break;
            default:
                StopMovement();
                break;
        }
    }

    private void StopMovement()
    {
        _playerBody.velocity = Vector2.zero;
    }
    
    private struct MovementInput
    {
        private bool _moveForward;
        private bool _moveBackward;
        private bool _moveRight;
        private bool _moveLeft;
        private bool _moveLeftUpward;
        private bool _moveLeftDownward;
        private bool _moveRightUpward;
        private bool _moveRightDownward;

        public MovementDirection GetCurrentInputs()
        {
            _moveForward = Input.GetKey(KeyCode.W);
            _moveBackward = Input.GetKey(KeyCode.S);
            _moveRight = Input.GetKey(KeyCode.D);
            _moveLeft = Input.GetKey(KeyCode.A);
            _moveLeftUpward = _moveForward && _moveLeft;
            _moveLeftDownward = _moveBackward && _moveLeft;
            _moveRightUpward = _moveForward && _moveRight;
            _moveRightDownward = _moveBackward && _moveRight;

            if (_moveLeft && _moveRight || _moveForward && _moveBackward)
            {
                return MovementDirection.Idle;
            }

            if (_moveLeftUpward)
            {
                return MovementDirection.LeftUp;
            }

            if (_moveRightUpward)
            {
                return MovementDirection.RightUp;
            }

            if (_moveLeftDownward)
            {
                return MovementDirection.LeftDown;
            }

            if (_moveRightDownward)
            {
                return MovementDirection.RightDown;
            }

            if (_moveForward)
            {
                return MovementDirection.Upward;
            }

            if (_moveBackward)
            {
                return MovementDirection.Downward;
            }

            if (_moveLeft)
            {
                return MovementDirection.Left;
            }

            if (_moveRight)
            {
                return MovementDirection.Right;
            }

            return MovementDirection.Idle;
        }
    }

    public enum MovementDirection
    {
        Idle,
        Upward,
        Downward,
        Left,
        Right,
        RightUp,
        LeftUp,
        RightDown,
        LeftDown
    }
}