using System.Collections.Generic;

namespace Arcomage.Entity.Interfaces
{
    public interface IStartParams
    {
        int MaxPlayerCard { get; set; }


        /// <summary>
        /// ����������� �������� ��� ������:
        /// �����, �����, �����, �������
        /// </summary>
        /// <returns></returns>
        Dictionary<Attributes, int> DefaultParams { get; set; }
    }
}
