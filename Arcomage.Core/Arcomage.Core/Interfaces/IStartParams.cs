using System.Collections.Generic;
using Arcomage.Entity;

namespace Arcomage.Core.Interfaces
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
