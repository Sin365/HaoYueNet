using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoYueNet.ServerNetwork.NetWork
{
	public static class ArrayPoolManager
	{
		static ArrayPool<byte> instance = ArrayPool<byte>.Shared;

		/// <summary>
		/// 租用指定大小byte数组
		/// </summary>
		/// <param name="lenght"></param>
		/// <returns></returns>
		public static byte[] RentByteArr(int lenght)
		{
			return instance.Rent(lenght);
		}


		/// <summary>
		/// 将数组归还给池  
		/// </summary>
		/// <param name="lenght"></param>
		/// <returns></returns>
		public static void ReturnByteArr(byte[] byteArr)
		{
			instance.Return(byteArr);
		}
	}
}
