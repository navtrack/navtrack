using System.Collections.Generic;
// ReSharper disable StringLiteralTypo

namespace Navtrack.Listener.Helpers.Crc;

public static class CrcStdParams
{
    public static Parameters Get(CrcAlgorithm algorithm) => Dictionary[algorithm];
    public static IEnumerable<Parameters> GetAll() => Dictionary.Values;
        
    private static readonly Dictionary<CrcAlgorithm, Parameters> Dictionary = new()
    {
            //CRC-8
            {CrcAlgorithm.Crc8, new Parameters("CRC-8", 8, 0x7, 0x0, false, false, 0x0, 0xF4)},
            {CrcAlgorithm.Crc8Cdma2000, new Parameters("CRC-8/CDMA2000", 8, 0x9B, 0xFF, false, false, 0x0, 0xDA)},
            {CrcAlgorithm.Crc8Darc, new Parameters("CRC-8/DARC", 8, 0x39, 0x0, true, true, 0x0, 0x15)},
            {CrcAlgorithm.Crc8DvbS2, new Parameters("CRC-8/DVB-S2", 8, 0xD5, 0x0, false, false, 0x0, 0xBC)},
            {CrcAlgorithm.Crc8Ebu, new Parameters("CRC-8/EBU", 8, 0x1D, 0xFF, true, true, 0x0, 0x97)},
            {CrcAlgorithm.Crc8ICode, new Parameters("CRC-8/I-CODE", 8, 0x1D, 0xFD, false, false, 0x0, 0x7E)},
            {CrcAlgorithm.Crc8Itu, new Parameters("CRC-8/ITU", 8, 0x7, 0x0, false, false, 0x55, 0xA1)},
            {CrcAlgorithm.Crc8Maxim, new Parameters("CRC-8/MAXIM", 8, 0x31, 0x0, true, true, 0x0, 0xA1)},
            {CrcAlgorithm.Crc8Rohc, new Parameters("CRC-8/ROHC", 8, 0x7, 0xFF, true, true, 0x0, 0xD0)},
            {CrcAlgorithm.Crc8Wcdma, new Parameters("CRC-8/WCDMA", 8, 0x9B, 0x0, true, true, 0x0, 0x25)},

            //CRC-10
            {CrcAlgorithm.Crc10, new Parameters("CRC-10", 10, 0x233, 0x0, false, false, 0x0, 0x199)},
            {
                CrcAlgorithm.Crc10Cdma2000,
                new Parameters("CRC-10/CDMA2000", 10, 0x3D9, 0x3FF, false, false, 0x0, 0x233)
            },

            //CRC-11
            {CrcAlgorithm.Crc11, new Parameters("CRC-11", 11, 0x385, 0x1A, false, false, 0x0, 0x5A3)},

            //CRC-12
            {CrcAlgorithm.Crc123Gpp, new Parameters("CRC-12/3GPP", 12, 0x80F, 0x0, false, true, 0x0, 0xDAF)},
            {
                CrcAlgorithm.Crc12Cdma2000,
                new Parameters("CRC-12/CDMA2000", 12, 0xF13, 0xFFF, false, false, 0x0, 0xD4D)
            },
            {CrcAlgorithm.Crc12Dect, new Parameters("CRC-12/DECT", 12, 0x80F, 0x0, false, false, 0x0, 0xF5B)},

            //CRC-13
            {CrcAlgorithm.Crc13Bbc, new Parameters("CRC-13/BBC", 13, 0x1CF5, 0x0, false, false, 0x0, 0x4FA)},

            //CRC-14
            {CrcAlgorithm.Crc14Darc, new Parameters("CRC-14/DARC", 14, 0x805, 0x0, true, true, 0x0, 0x82D)},

            //CRC-15
            {CrcAlgorithm.Crc15, new Parameters("CRC-15", 15, 0x4599, 0x0, false, false, 0x0, 0x59E)},
            {
                CrcAlgorithm.Crc15Mpt1327,
                new Parameters("CRC-15/MPT1327", 15, 0x6815, 0x0, false, false, 0x1, 0x2566)
            },

            //CRC-16
            {
                CrcAlgorithm.Crc16CcittFalse,
                new Parameters("CRC-16/CCITT-FALSE", 16, 0x1021, 0xFFFF, false, false, 0x0, 0x29B1)
            },
            {CrcAlgorithm.Crc16Arc, new Parameters("CRC-16/ARC", 16, 0x8005, 0x0, true, true, 0x0, 0xBB3D)},
            {
                CrcAlgorithm.Crc16AugCcitt,
                new Parameters("CRC-16/AUG-CCITT", 16, 0x1021, 0x1D0F, false, false, 0x0, 0xE5CC)
            },
            {
                CrcAlgorithm.Crc16Buypass,
                new Parameters("CRC-16/BUYPASS", 16, 0x8005, 0x0, false, false, 0x0, 0xFEE8)
            },
            {
                CrcAlgorithm.Crc16Cdma2000,
                new Parameters("CRC-16/CDMA2000", 16, 0xC867, 0xFFFF, false, false, 0x0, 0x4C06)
            },
            {
                CrcAlgorithm.Crc16Dds110,
                new Parameters("CRC-16/DDS-110", 16, 0x8005, 0x800D, false, false, 0x0, 0x9ECF)
            },
            {CrcAlgorithm.Crc16DectR, new Parameters("CRC-16/DECT-R", 16, 0x589, 0x0, false, false, 0x1, 0x7E)},
            {CrcAlgorithm.Crc16DectX, new Parameters("CRC-16/DECT-X", 16, 0x589, 0x0, false, false, 0x0, 0x7F)},
            {CrcAlgorithm.Crc16Dnp, new Parameters("CRC-16/DNP", 16, 0x3D65, 0x0, true, true, 0xFFFF, 0xEA82)},
            {
                CrcAlgorithm.Crc16En13757,
                new Parameters("CRC-16/EN-13757", 16, 0x3D65, 0x0, false, false, 0xFFFF, 0xC2B7)
            },
            {
                CrcAlgorithm.Crc16Genibus,
                new Parameters("CRC-16/GENIBUS", 16, 0x1021, 0xFFFF, false, false, 0xFFFF, 0xD64E)
            },
            {CrcAlgorithm.Crc16Maxim, new Parameters("CRC-16/MAXIM", 16, 0x8005, 0x0, true, true, 0xFFFF, 0x44C2)},
            {
                CrcAlgorithm.Crc16Mcrf4Xx,
                new Parameters("CRC-16/MCRF4XX", 16, 0x1021, 0xFFFF, true, true, 0x0, 0x6F91)
            },
            {
                CrcAlgorithm.Crc16Riello,
                new Parameters("CRC-16/RIELLO", 16, 0x1021, 0xB2AA, true, true, 0x0, 0x63D0)
            },
            {
                CrcAlgorithm.Crc16T10Dif,
                new Parameters("CRC-16/T10-DIF", 16, 0x8BB7, 0x0, false, false, 0x0, 0xD0DB)
            },
            {
                CrcAlgorithm.Crc16Teledisk,
                new Parameters("CRC-16/TELEDISK", 16, 0xA097, 0x0, false, false, 0x0, 0xFB3)
            },
            {
                CrcAlgorithm.Crc16Tms37157,
                new Parameters("CRC-16/TMS37157", 16, 0x1021, 0x89EC, true, true, 0x0, 0x26B1)
            },
            {CrcAlgorithm.Crc16Usb, new Parameters("CRC-16/USB", 16, 0x8005, 0xFFFF, true, true, 0xFFFF, 0xB4C8)},
            {CrcAlgorithm.CrcA, new Parameters("CRC-A", 16, 0x1021, 0xC6C6, true, true, 0x0, 0xBF05)},
            {CrcAlgorithm.Crc16Kermit, new Parameters("CRC-16/KERMIT", 16, 0x1021, 0x0, true, true, 0x0, 0x2189)},
            {
                CrcAlgorithm.Crc16Modbus,
                new Parameters("CRC-16/MODBUS", 16, 0x8005, 0xFFFF, true, true, 0x0, 0x4B37)
            },
            {CrcAlgorithm.Crc16X25, new Parameters("CRC-16/X-25", 16, 0x1021, 0xFFFF, true, true, 0xFFFF, 0x906E)},
            {
                CrcAlgorithm.Crc16Xmodem,
                new Parameters("CRC-16/XMODEM", 16, 0x1021, 0x0, false, false, 0x0, 0x31C3)
            },

            //CRC-24
            {CrcAlgorithm.Crc24, new Parameters("CRC-24", 24, 0x864CFB, 0xB704CE, false, false, 0x0, 0x21CF02)},
            {
                CrcAlgorithm.Crc24FlexrayA,
                new Parameters("CRC-24/FLEXRAY-A", 24, 0x5D6DCB, 0xFEDCBA, false, false, 0x0, 0x7979BD)
            },
            {
                CrcAlgorithm.Crc24FlexrayB,
                new Parameters("CRC-24/FLEXRAY-B", 24, 0x5D6DCB, 0xABCDEF, false, false, 0x0, 0x1F23B8)
            },

            //CRC-31
            {
                CrcAlgorithm.Crc31Philips,
                new Parameters("CRC-31/PHILIPS", 31, 0x4C11DB7, 0x7FFFFFFF, false, false, 0x7FFFFFFF, 0xCE9E46C)
            },

            //CRC-32
            {
                CrcAlgorithm.Crc32,
                new Parameters("CRC-32", 32, 0x04C11DB7, 0xFFFFFFFF, true, true, 0xFFFFFFFF, 0xCBF43926)
            },
            {
                CrcAlgorithm.Crc32Bzip2,
                new Parameters("CRC-32/BZIP2", 32, 0x04C11DB7, 0xFFFFFFFF, false, false, 0xFFFFFFFF, 0xFC891918)
            },
            {
                CrcAlgorithm.Crc32C,
                new Parameters("CRC-32C", 32, 0x1EDC6F41, 0xFFFFFFFF, true, true, 0xFFFFFFFF, 0xE3069283)
            },
            {
                CrcAlgorithm.Crc32D,
                new Parameters("CRC-32D", 32, 0xA833982B, 0xFFFFFFFF, true, true, 0xFFFFFFFF, 0x87315576)
            },
            {
                CrcAlgorithm.Crc32Jamcrc,
                new Parameters("CRC-32/JAMCRC", 32, 0x04C11DB7, 0xFFFFFFFF, true, true, 0x00000000, 0x340BC6D9)
            },
            {
                CrcAlgorithm.Crc32Mpeg2,
                new Parameters("CRC-32/MPEG-2", 32, 0x04C11DB7, 0xFFFFFFFF, false, false, 0x00000000, 0x0376E6E7)
            },
            {
                CrcAlgorithm.Crc32Posix,
                new Parameters("CRC-32/POSIX", 32, 0x04C11DB7, 0x00000000, false, false, 0xFFFFFFFF, 0x765E7680)
            },
            {
                CrcAlgorithm.Crc32Q,
                new Parameters("CRC-32Q", 32, 0x814141AB, 0x00000000, false, false, 0x00000000, 0x3010BF7F)
            },
            {
                CrcAlgorithm.Crc32Xfer,
                new Parameters("CRC-32/XFER", 32, 0x000000AF, 0x00000000, false, false, 0x00000000, 0xBD0BE338)
            },

            //CRC-40
            {
                CrcAlgorithm.Crc40Gsm,
                new Parameters("CRC-40/GSM", 40, 0x4820009, 0x0, false, false, 0xFFFFFFFFFF, 0xD4164FC646)
            },

            //CRC-64
            {
                CrcAlgorithm.Crc64,
                new Parameters("CRC-64", 64, 0x42F0E1EBA9EA3693, 0x00000000, false, false, 0x00000000,
                    0x6C40DF5F0B497347)
            },
            {
                CrcAlgorithm.Crc64We,
                new Parameters("CRC-64/WE", 64, 0x42F0E1EBA9EA3693, 0xFFFFFFFFFFFFFFFF, false, false,
                    0xFFFFFFFFFFFFFFFF, 0x62EC59E3F1A4F00A)
            },
            {
                CrcAlgorithm.Crc64Xz,
                new Parameters("CRC-64/XZ", 64, 0x42F0E1EBA9EA3693, 0xFFFFFFFFFFFFFFFF, true, true,
                    0xFFFFFFFFFFFFFFFF, 0x995DC9BBDF1939FA)
            }
        };
}