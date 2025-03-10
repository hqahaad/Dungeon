public interface IOperationStrategy
{
    float Culculate(float value);
}


public class AddOperation : IOperationStrategy
{
    private readonly float value;

    public AddOperation(float value)
    {
        this.value = value;
    }

    public float Culculate(float value) => this.value + value;
}

public class MultiplyOperation : IOperationStrategy
{
    private readonly float value;

    public MultiplyOperation(float value)
    {
        this.value = value;
    }

    public float Culculate(float value) => this.value * value;
}