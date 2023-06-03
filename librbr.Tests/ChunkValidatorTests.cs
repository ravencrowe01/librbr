namespace librbr.Tests;

using librbr.Framework.World;
using librbr.Framework.World.Chunk;
using librbr.World;
using librbr.World.Chunk;
using Raven.Util;

public class Tests {
    private IChunkConfig _chunkA;

    [SetUp]
    public void Setup ( ) {
        SetupChunkA();
    }

    [Test]
    public void Test1 ( ) {
        var val = new ChunkValidator();

        Assert.That(val.ValidateChunk(_chunkA), Is.EqualTo(true));
    }

    private void SetupChunkA ( ) {
        _chunkA = new ProtoChunk(4, new Coordinate(0, 0));

        var r00 = new ProtoRoom(new Coordinate(0, 0));
        r00.OpenSide(Direction.North);
        r00.OpenSide(Direction.East);
        _chunkA.SetRoom(new Coordinate(0, 0), r00);

        var r10 = new ProtoRoom(new Coordinate(1, 0));
        r10.SetSideState(Direction.West, true);
        _chunkA.SetRoom(new Coordinate(1, 0), r10);

        var r20 = new ProtoRoom(new Coordinate(2, 0));
        r20.OpenSide(Direction.North);
        _chunkA.SetRoom(new Coordinate(2, 0), r20);

        var r30 = new ProtoRoom(new Coordinate(3, 0));
        r30.OpenSide(Direction.North);
        _chunkA.SetRoom(new Coordinate(3, 0), r30);

        var r01 = new ProtoRoom(new Coordinate(0, 1));
        r01.OpenSide(Direction.East);
        r01.OpenSide(Direction.South);
        _chunkA.SetRoom(new Coordinate(0, 1), r01);

        var r11 = new ProtoRoom(new Coordinate(1, 1));
        r11.OpenSide(Direction.North);
        r11.OpenSide(Direction.East);
        r11.OpenSide(Direction.West);
        _chunkA.SetRoom(new Coordinate(1, 1), r11);

        var r21 = new ProtoRoom(new Coordinate(2, 1));
        r21.OpenSide(Direction.South);
        r21.OpenSide(Direction.East);
        r21.OpenSide(Direction.West);
        _chunkA.SetRoom(new Coordinate(2, 1), r21);

        var r31 = new ProtoRoom(new Coordinate(3, 1));
        r31.OpenSide(Direction.North);
        r31.OpenSide(Direction.South);
        r31.OpenSide(Direction.West);
        _chunkA.SetRoom(new Coordinate(3, 1), r31);

        var r02 = new ProtoRoom(new Coordinate(0, 2));
        r02.OpenSide(Direction.East);
        _chunkA.SetRoom(new Coordinate(0, 2), r02);

        var r12 = new ProtoRoom(new Coordinate(1, 2));
        r12.OpenSide(Direction.North);
        r12.OpenSide(Direction.South);
        r12.OpenSide(Direction.East);
        _chunkA.SetRoom(new Coordinate(1, 2), r12);

        var r22 = new ProtoRoom(new Coordinate(2, 2));
        r22.OpenSide(Direction.North);
        r22.OpenSide(Direction.East);
        _chunkA.SetRoom(new Coordinate(2, 2), r22);

        var r32 = new ProtoRoom(new Coordinate(3, 2));
        r32.OpenSide(Direction.South);
        r32.OpenSide(Direction.West);
        _chunkA.SetRoom(new Coordinate(3, 2), r32);

        var r03 = new ProtoRoom(new Coordinate(0, 3));
        r03.OpenSide(Direction.East);
        _chunkA.SetRoom(new Coordinate(0, 3), r03);

        var r13 = new ProtoRoom(new Coordinate(1, 3));
        r13.OpenSide(Direction.South);
        r13.OpenSide(Direction.West);
        _chunkA.SetRoom(new Coordinate(1, 3), r13);

        var r23 = new ProtoRoom(new Coordinate(2, 3));
        r23.OpenSide(Direction.South);
        r23.OpenSide(Direction.East);
        _chunkA.SetRoom(new Coordinate(2, 3), r23);

        var r33 = new ProtoRoom(new Coordinate(3, 3));
        r33.OpenSide(Direction.West);
        _chunkA.SetRoom(new Coordinate(3, 3), r33);
    }
}