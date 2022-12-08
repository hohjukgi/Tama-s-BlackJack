﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Tama_s_BlackJack
{
    class RateManager
    {
        private int _rate;
        private const int count = 9;
        public readonly int interval = 400;
        private readonly int MaxRate;
        public bool enableRateBenalty = false;
        public int rate
        {
            get
            {
                return this._rate;
            }
        }
        private int _rateBef;
        public int rateBef
        {
            get
            {
                return _rateBef;
            }
        }

        private Encryption encrypt = new Encryption();
        
        public RateManager()
        {
            _rateBef = 0;
            MaxRate = interval * count - 1;
            encrypt.fileName = "rateData.dat";
            try
            {
                List<String> data = encrypt.Decrypt();
                this._rate = int.Parse(data[data.Count - 1]);
            }
            catch (FileNotFoundException)
            {
                this._rate = 0;
            }
        }

        /// <summary>
        /// 切断時ペナルティを追加
        /// </summary>
        public void SetRatePenalty()
        {
            if (!enableRateBenalty) return;
            //切断時ペナルティ
            int ratePenalty = this.rate;
            ratePenalty -= 100;
            if (ratePenalty < 0) ratePenalty = 0;
            encrypt.Encrypt(ratePenalty.ToString());
        }

        public void CalcRate(float tScore)
        {
            _rateBef = rate;
            _rate += (int)(tScore - _rate) / 3;
            if(_rate > MaxRate)_rate = MaxRate;
            encrypt.Encrypt(this._rate.ToString());
        }

    }
}
