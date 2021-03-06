﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using SystemHalf;

namespace TpacTool.Lib
{
	public class VertexStreamData : AbstractMeshData
	{
		public static readonly Guid TYPE_GUID = Guid.Parse("bb1df897-584f-4770-abf2-663fe449f247");

		public const string KEY_IS_32BIT_INDEX = "is32bit";

		public int[] Indices { set; get; }

		public Color[] Colors1 { set; get; }

		public Color[] Colors2 { set; get; }

		public Vector2[] Uv1 { set; get; }

		public Vector2[] Uv2 { set; get; }

		public Vector3[] Positions { set; get; }

		public Vector3[] UnknownAnotherPositions { set; get; }

		public Vector3[] Normals { set; get; }

		public Vector4[] Tangents { set; get; }

		public BoneWeight[] BoneWeights { set; get; }

		public BoneIndex[] BoneIndices { set; get; }

		public X11Y11Z10[] CompressedNormals { set; get; }

		public Half4[] CompressedPositions { set; get; }

		public X10Y11Z10W1[] CompressedTangents { set; get; }

		public VertexStreamData() : base(TYPE_GUID)
		{
			Indices = CreateEmptyArray<int>();
		}

		public override void ReadData(BinaryReader stream, IDictionary<object, object> userdata, int totalSize)
		{
			bool use32bit = userdata != null && userdata.TryGetValue(KEY_IS_32BIT_INDEX, out var value) && true.Equals(value);
			int indexNum = stream.ReadInt32();
			if (use32bit)
			{
				Indices = ReadStructArray<int>(stream, indexNum);
			}
			else
			{
				int[] temp = new int[indexNum];
				for (int i = 0; i < indexNum; i++)
				{
					temp[i] = stream.ReadUInt16();
				}
				Indices = temp;
			}
			var sizes = ReadStructArray<ulong>(stream, 26);
			Colors1 = ReadStructArray<Color>(stream, (int)(sizes[1] / 4));
			Colors2 = ReadStructArray<Color>(stream, (int)(sizes[3] / 4));
			Uv1 = ReadStructArray<Vector2>(stream, (int) (sizes[5] / 8));
			Uv2 = ReadStructArray<Vector2>(stream, (int)(sizes[7] / 8));
			Positions = ReadStructArray<Vector3>(stream, (int)(sizes[9] / 12));
			UnknownAnotherPositions = ReadStructArray<Vector3>(stream, (int)(sizes[11] / 12));
			Normals = ReadStructArray<Vector3>(stream, (int)(sizes[13] / 12));
			Tangents = ReadStructArray<Vector4>(stream, (int)(sizes[15] / 16));
			BoneWeights = ReadStructArray<BoneWeight>(stream, (int)(sizes[19] / 4));
			BoneIndices = ReadStructArray<BoneIndex>(stream, (int)(sizes[17] / 4));
			CompressedNormals = ReadStructArray<X11Y11Z10>(stream, (int) (sizes[21] / 4));
			CompressedPositions = ReadStructArray<Half4>(stream, (int) (sizes[23] / 8));
			CompressedTangents = ReadStructArray<X10Y11Z10W1>(stream, (int) (sizes[25] / 4));
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct BoneIndex : IFormattable
		{
			public byte B1;
			public byte B2;
			public byte B3;
			public byte B4;

			public override string ToString()
			{
				return ToString("G", CultureInfo.CurrentCulture);
			}

			public string ToString(string format)
			{
				return ToString(format, CultureInfo.CurrentCulture);
			}

			public string ToString(string format, IFormatProvider formatProvider)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string numberGroupSeparator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
				stringBuilder.Append('<');
				stringBuilder.Append(((IFormattable)B1).ToString(format, formatProvider));
				stringBuilder.Append(numberGroupSeparator);
				stringBuilder.Append(' ');
				stringBuilder.Append(((IFormattable)B2).ToString(format, formatProvider));
				stringBuilder.Append(numberGroupSeparator);
				stringBuilder.Append(' ');
				stringBuilder.Append(((IFormattable)B3).ToString(format, formatProvider));
				stringBuilder.Append(numberGroupSeparator);
				stringBuilder.Append(' ');
				stringBuilder.Append(((IFormattable)B4).ToString(format, formatProvider));
				stringBuilder.Append('>');
				return stringBuilder.ToString();
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct BoneWeight : IFormattable
		{
			public byte W1;
			public byte W2;
			public byte W3;
			public byte W4;

			public override string ToString()
			{
				return ToString("G", CultureInfo.CurrentCulture);
			}

			public string ToString(string format)
			{
				return ToString(format, CultureInfo.CurrentCulture);
			}

			public string ToString(string format, IFormatProvider formatProvider)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string numberGroupSeparator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
				stringBuilder.Append('<');
				stringBuilder.Append(((IFormattable)(W1 / 255f)).ToString(format, formatProvider));
				stringBuilder.Append(numberGroupSeparator);
				stringBuilder.Append(' ');
				stringBuilder.Append(((IFormattable)(W2 / 255f)).ToString(format, formatProvider));
				stringBuilder.Append(numberGroupSeparator);
				stringBuilder.Append(' ');
				stringBuilder.Append(((IFormattable)(W3 / 255f)).ToString(format, formatProvider));
				stringBuilder.Append(numberGroupSeparator);
				stringBuilder.Append(' ');
				stringBuilder.Append(((IFormattable)(W4 / 255f)).ToString(format, formatProvider));
				stringBuilder.Append('>');
				return stringBuilder.ToString();
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Half4 : IFormattable
		{
			public Half X;
			public Half Y;
			public Half Z;
			public Half W;

			public override string ToString()
			{
				return ToString("G", CultureInfo.CurrentCulture);
			}

			public string ToString(string format)
			{
				return ToString(format, CultureInfo.CurrentCulture);
			}

			public string ToString(string format, IFormatProvider formatProvider)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string numberGroupSeparator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
				stringBuilder.Append('<');
				stringBuilder.Append(((IFormattable)X).ToString(format, formatProvider));
				stringBuilder.Append(numberGroupSeparator);
				stringBuilder.Append(' ');
				stringBuilder.Append(((IFormattable)Y).ToString(format, formatProvider));
				stringBuilder.Append(numberGroupSeparator);
				stringBuilder.Append(' ');
				stringBuilder.Append(((IFormattable)Z).ToString(format, formatProvider));
				stringBuilder.Append(numberGroupSeparator);
				stringBuilder.Append(' ');
				stringBuilder.Append(((IFormattable)W).ToString(format, formatProvider));
				stringBuilder.Append('>');
				return stringBuilder.ToString();
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct X11Y11Z10 : IFormattable
		{
			public uint RawData;

			public int RawZ
			{
				get { return (int)(RawData & 0x3FF); }
				set { RawData = (RawData & (~0x3FFu)) | ((uint)value & 0x3FFu); }
			}

			public int RawY
			{
				get { return (int)((RawData & 0x1FFC00) >> 10); }
				set { RawData = (RawData & (~0x1FFC00u)) | (((uint)value & 0x7FFu) << 10); }
			}

			public int RawX
			{
				get { return (int)((RawData & 0xFFE00000u) >> 21); }
				set { RawData = (RawData & (~0xFFE00000u)) | (((uint)value & 0x7FFu) << 21); }
			}

			public float Z
			{
				get { return (RawZ / 1023f) * 2f - 1; }
				set { RawZ = Math.Min((int)((value.Clamp() + 1) * 1023) / 2, 0x3FF); }
			}

			public float Y
			{
				get { return (RawY / 2047f) * 2f - 1; }
				set { RawY = Math.Min((int)((value.Clamp() + 1) * 2047) / 2, 0x7FF); }
			}

			public float X
			{
				get { return (RawX / 2047f) * 2f - 1; }
				set { RawX = Math.Min((int)((value.Clamp() + 1) * 2047) / 2, 0x7FF); }
			}

			public override string ToString()
			{
				return ToString("G", CultureInfo.CurrentCulture);
			}

			public string ToString(string format)
			{
				return ToString(format, CultureInfo.CurrentCulture);
			}

			public string ToString(string format, IFormatProvider formatProvider)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string numberGroupSeparator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
				stringBuilder.Append('<');
				stringBuilder.Append(((IFormattable)X).ToString(format, formatProvider));
				stringBuilder.Append(numberGroupSeparator);
				stringBuilder.Append(' ');
				stringBuilder.Append(((IFormattable)Y).ToString(format, formatProvider));
				stringBuilder.Append(numberGroupSeparator);
				stringBuilder.Append(' ');
				stringBuilder.Append(((IFormattable)Z).ToString(format, formatProvider));
				stringBuilder.Append('>');
				return stringBuilder.ToString();
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct X10Y11Z10W1 : IFormattable
		{
			public uint RawData;

			public int RawZ
			{
				get { return (int)(RawData & 0x3FF); }
				set { RawData = (RawData & (~0x3FFu)) | ((uint)value & 0x3FFu); }
			}

			public int RawY
			{
				get { return (int)((RawData & 0x1FFC00) >> 10); }
				set { RawData = (RawData & (~0x1FFC00u)) | (((uint)value & 0x7FFu) << 10); }
			}

			public int RawX
			{
				get { return (int)((RawData & 0x7FE00000) >> 21); }
				set { RawData = (RawData & (~0x7FE00000u)) | (((uint)value & 0x3FFu) << 21); }
			}

			public int RawW
			{
				get { return (int)((RawData & 0x80000000u) >> 31); }
				set { RawData = (RawData & (~0x80000000u)) | (value > 0 ? 0x80000000u : 0); }
			}

			public float Z
			{
				get { return (RawZ / 1023f) * 2f - 1; }
				set { RawZ = Math.Min((int)((value.Clamp() + 1) * 1023) / 2, 0x3FF); }
			}

			public float Y
			{
				get { return (RawY / 2047f) * 2f - 1; }
				set { RawY = Math.Min((int)((value.Clamp() + 1) * 2047) / 2, 0x7FF); }
			}

			public float X
			{
				get { return (RawX / 1023f) * 2f - 1; }
				set { RawX = Math.Min((int)((value.Clamp() + 1) * 1023) / 2, 0x3FF); }
			}

			public bool IsNeg
			{
				get { return RawW == 1; }
				set { RawW = IsNeg ? 1 : 0; }
			}

			public int Sign
			{
				get { return IsNeg ? -1 : 1; }
				set { IsNeg = value < 0; }
			}


			public override string ToString()
			{
				return ToString("G", CultureInfo.CurrentCulture);
			}

			public string ToString(string format)
			{
				return ToString(format, CultureInfo.CurrentCulture);
			}

			public string ToString(string format, IFormatProvider formatProvider)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string numberGroupSeparator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
				stringBuilder.Append('<');
				stringBuilder.Append(((IFormattable)X).ToString(format, formatProvider));
				stringBuilder.Append(numberGroupSeparator);
				stringBuilder.Append(' ');
				stringBuilder.Append(((IFormattable)Y).ToString(format, formatProvider));
				stringBuilder.Append(numberGroupSeparator);
				stringBuilder.Append(' ');
				stringBuilder.Append(((IFormattable)Z).ToString(format, formatProvider));
				stringBuilder.Append(numberGroupSeparator);
				stringBuilder.Append(' ');
				stringBuilder.Append(Sign.ToString(format, formatProvider));
				stringBuilder.Append('>');
				return stringBuilder.ToString();
			}
		}
	}
}