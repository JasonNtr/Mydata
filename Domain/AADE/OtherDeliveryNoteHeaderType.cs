using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AADE
{
    public partial class OtherDeliveryNoteHeaderType
    {

        private AddressType loadingAddressField;

        private AddressType deliveryAddressField;

        private int startShippingBranchField;

        private bool startShippingBranchFieldSpecified;

        private int completeShippingBranchField;

        private bool completeShippingBranchFieldSpecified;

        /// <remarks/>
        public AddressType loadingAddress
        {
            get
            {
                return this.loadingAddressField;
            }
            set
            {
                this.loadingAddressField = value;
            }
        }

        /// <remarks/>
        public AddressType deliveryAddress
        {
            get
            {
                return this.deliveryAddressField;
            }
            set
            {
                this.deliveryAddressField = value;
            }
        }

        /// <remarks/>
        public int startShippingBranch
        {
            get
            {
                return this.startShippingBranchField;
            }
            set
            {
                this.startShippingBranchField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool startShippingBranchSpecified
        {
            get
            {
                return this.startShippingBranchFieldSpecified;
            }
            set
            {
                this.startShippingBranchFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int completeShippingBranch
        {
            get
            {
                return this.completeShippingBranchField;
            }
            set
            {
                this.completeShippingBranchField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool completeShippingBranchSpecified
        {
            get
            {
                return this.completeShippingBranchFieldSpecified;
            }
            set
            {
                this.completeShippingBranchFieldSpecified = value;
            }
        }
    }
}
