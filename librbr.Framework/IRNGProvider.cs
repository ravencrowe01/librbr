namespace librbr.Framework {
    public interface IRNGProvider {
        int Range (int max);

        int Range (int min, int max);

        float Range (float max);

        float Range (float min, float max);

        void SetSeed (int seed);
    }
}