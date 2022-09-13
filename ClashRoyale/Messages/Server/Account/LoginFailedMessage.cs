﻿namespace ClashRoyale.Messages.Server.Account
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Files;

    using Newtonsoft.Json;

    public class LoginFailedMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20103;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        /// <summary>
        /// Gets the patching host according to the specified reason.
        /// </summary>
        private string ContentUrl
        {
            get
            {
                if (Fingerprint.IsCustom)
                {
                    return "https://raw.githubusercontent.com/BerkanYildiz/Patchs/master/ClashRoyale";
                }

                return "http://7166046b142482e67b30-2a63f4436c967aa7d355061bd0d924a1.r65.cf1.rackcdn.com";
            }
        }

        /// <summary>
        /// Gets the assets URL according to the specified reason.
        /// </summary>
        private string AssetsUrl
        {
            get
            {
                if (Fingerprint.IsCustom)
                {
                    return "https://game-assets.clashroyaleapp.com";
                }

                return "https://game-assets.clashroyaleapp.com";
            }
        }

        /// <summary>
        /// Gets the content update according to the specified reason.
        /// </summary>
        private string ContentUpdate
        {
            get
            {
                if (this.Reason == Reason.Patch)
                {
                    return Fingerprint.Json.ToString(Formatting.None);
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the redirect domain according to the specified reason.
        /// </summary>
        private string RedirectDomain
        {
            get
            {
                if (this.Reason == Reason.Redirection)
                {
                    return "game.clashroyaleapp.com";
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the update URL according to the specified reason.
        /// </summary>
        private string UpdateUrl
        {
            get
            {
                if (this.Reason == Reason.Update)
                {
                    return "https://mega.nz/#!E90UwDKa!e6Xv7bEXWSJDCyQy2o1PNqxONh0Q4qt3_rM5cks-pMo";
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the time left according to the specified reason.
        /// </summary>
        private int TimeLeft
        {
            get
            {
                if (this.Reason == Reason.Maintenance)
                {
                    return 3600;
                }

                if (this.Reason == Reason.Banned)
                {
                    return 604800;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the message according to the specified reason.
        /// </summary>
        private string Message
        {
            get
            {
                if (this.Reason == Reason.Default)
                {
                    return "GobelinLand";
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the game urls.
        /// </summary>
        private string[] GameUrls
        {
            get
            {
                return new[]
                {
                    this.AssetsUrl, this.ContentUrl
                };
            }
        }

        public Reason Reason;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginFailedMessage"/> class.
        /// </summary>
        public LoginFailedMessage()
        {
            // LoginFailedMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginFailedMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public LoginFailedMessage(ByteStream Stream) : base(Stream)
        {
            // LoginFailedMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginFailedMessage"/> class.
        /// </summary>
        /// <param name="Reason">The reason.</param>
        public LoginFailedMessage(Reason Reason = Reason.Default)
        {
            this.Version = 4;
            this.Reason  = Reason;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Reason  = (Reason) this.Stream.ReadVInt();

            this.Stream.ReadString();
            this.Stream.ReadString();
            this.Stream.ReadString();
            this.Stream.ReadString();

            this.Stream.ReadVInt();
            this.Stream.ReadBoolean();

            this.Stream.ReadString();

            for (int i = 0; i < this.Stream.ReadVInt(); i++)
            {
                this.Stream.ReadString();
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt((int) this.Reason);

            this.Stream.WriteString(this.ContentUpdate);
            this.Stream.WriteString(this.UpdateUrl);
            this.Stream.WriteString(this.Message);
            this.Stream.WriteString(this.ContentUrl);

            this.Stream.WriteVInt(this.TimeLeft);
            this.Stream.WriteBoolean(false);

            this.Stream.WriteString(null);

            // Game Urls

            this.Stream.WriteVInt(this.GameUrls.Length);

            foreach (string GameUrl in this.GameUrls)
            {
                this.Stream.WriteString(GameUrl);
            }
        }
    }
}