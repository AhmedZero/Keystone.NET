// See https://aka.ms/new-console-template for more information
using keystone.NET;
namespace Test // Note: actual namespace depends on the project name.
{
    public unsafe class Program
    {
        public static void Main(string[] args)
        {
            var CODE = "CALL [55555]";
            KsStruct ks;
            KsErr err;
            ulong count;
            byte* encode;
            ulong size;

            err = keystone.NET.keystone.KsOpen(KsArch.KS_ARCH_X86, (int)KsMode.KS_MODE_64, out ks);
            if (err != KsErr.KS_ERR_OK)
            {
                Console.WriteLine("ERROR: failed on ks_open(), quit\n");
            }
            if (keystone.NET.keystone.KsAsm(ks, CODE, 0, &encode, out size, out count) != (int)KsErr.KS_ERR_OK)
            {
                Console.WriteLine("ERROR: ks_asm() failed & count = %lu, error = %u\n",
                       count, keystone.NET.keystone.KsErrno(ks));
            }
            else
            {
                ulong i;

                Console.WriteLine($"{CODE} = ");
                for (i = 0; i < size; i++)
                {
                    Console.WriteLine($"{encode[i].ToString("X")} ");
                }
                Console.WriteLine("\n");
                Console.WriteLine($"Compiled: {size} bytes, statements: {count}\n");
            }

            // NOTE: free encode after usage to avoid leaking memory
            keystone.NET.keystone.KsFree(encode);

            // close Keystone instance when done
            keystone.NET.keystone.KsClose(ks);
        }
    }
}
