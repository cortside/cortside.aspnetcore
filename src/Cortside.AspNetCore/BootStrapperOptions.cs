using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cortside.Common.BootStrap;

namespace Cortside.AspNetCore {
    public class BootStrapperOptions {
        protected List<IInstaller> installers;

        public BootStrapperOptions() {
            installers = new List<IInstaller>();
        }

        public void AddInstaller(IInstaller installer) {
            installers.Add(installer);
        }

        public ReadOnlyCollection<IInstaller> Installers {
            get { return installers.AsReadOnly(); }
        }
    }
}
