using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace GDAPSIIGame.Audio
{
	class AudioManager
	{
		//Fields
		private static AudioManager instance;
		private Dictionary<String, SoundEffect> soundEffects;

		/// <summary>
		/// Singleton access
		/// </summary>
		/// <returns></returns>
		static public AudioManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new AudioManager();
				}
				return instance;
			}
		}

		private AudioManager()
		{
			soundEffects = new Dictionary<string, SoundEffect>();
		}

		public void LoadContent(ContentManager Content)
		{
			soundEffects.Add("Blip", Content.Load<SoundEffect>("SoundEffects\\blip"));
			soundEffects.Add("Hurt", Content.Load<SoundEffect>("SoundEffects\\hurt"));
			soundEffects.Add("DamageSound", Content.Load<SoundEffect>("SoundEffects\\damagesound"));

            soundEffects.Add("PistolShoot", Content.Load<SoundEffect>("SoundEffects\\PistolShoot"));
            soundEffects.Add("ShotgunShoot", Content.Load<SoundEffect>("SoundEffects\\ShotgunShoot"));
            soundEffects.Add("RifleShoot", Content.Load<SoundEffect>("SoundEffects\\RifleShoot"));
        }

		public SoundEffect GetSoundEffect(String name)
		{
			if (soundEffects.ContainsKey(name))
			{
				return soundEffects[name];
			}
			else return null;
		}
	}
}
