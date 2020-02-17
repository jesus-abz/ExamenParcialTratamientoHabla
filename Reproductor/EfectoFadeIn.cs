using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Wave;

namespace Reproductor
{
    class EfectoFadeIn : ISampleProvider
    {
        private ISampleProvider fuente;
        private int muestrasLeidas = 0;
        private float segundosTranscurridos = 0;
        private float inicio;
        private float duracion;

        public EfectoFadeIn(ISampleProvider fuente, float inicio, float duracion)
        {
            this.fuente = fuente;
            this.inicio = inicio;
            this.duracion = duracion;
        }

        public WaveFormat WaveFormat
        {
            get
            {
                return fuente.WaveFormat;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int read = fuente.Read(buffer, offset, count);

            
            muestrasLeidas += read;
            segundosTranscurridos = (float)muestrasLeidas / (float)fuente.WaveFormat.SampleRate / (float)fuente.WaveFormat.Channels;

            if (segundosTranscurridos >= inicio && segundosTranscurridos <= inicio + duracion)
            {
                //Aplicar el efecto
                float factorEscala = (segundosTranscurridos - inicio) / duracion;
                for (int i = 0; i < read; i++)
                {
                    buffer[i + offset] *= factorEscala;
                }
            }
            else if (segundosTranscurridos <= inicio)
            {
                for (int i = 0; i < read; i++)
                {
                    buffer[i + offset] = 0.0f;
                }
            }

            return read;
        }
    }
}
