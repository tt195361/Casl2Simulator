namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令オペランドでオペランドなしを表わします。
    /// </summary>
    internal class NoOperand : MachineInstructionOperand
    {
        internal static readonly NoOperand Instance = new NoOperand();

        private NoOperand()
        {
            //
        }
    }
}
