namespace librbr.Tests {
    public partial class ChunkValidatorTests {
        private ProtoChunk _chunkA;

        [SetUp]
        public void Setup ( ) {
            SetupChunkA();
        }

        [Test]
        public void ChunkValidator_ChunkA_Valid ( ) {
            var val = new ChunkValidator();

            // System.Console.WriteLine(_chunkA.ToString());

            Assert.That(val.ValidateChunk(_chunkA), Is.EqualTo(true));
        }
    }
}
