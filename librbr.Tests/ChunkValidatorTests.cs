namespace librbr.Tests;

using librbr.Framework.World.Chunk;
using librbr.World;

public class Tests {
    private IChunkConfig _chunkA;

    [SetUp]
    public void Setup ( ) {
        _chunkA = new ProtoChunk(3);
    }

    [Test]
    public void Test1 ( ) {
        Assert.Pass();
    }
}