namespace SimpleJSON
{
	public abstract partial class JSONNode
	{
		public bool tryFetch(string pos, ref string resp) {
			if(this[pos] != null && this[pos].IsString) {
				resp = this[pos].Value;
				return true;
			}
			return false;
		}
		public bool tryFetch(int pos, ref string resp) {
			if(this[pos] != null && this[pos].IsString) {
				resp = this[pos].Value;
				return true;
			}
			return false;
		}

		public bool tryFetch(string pos, ref double resp) {
			if(this[pos] != null && this[pos].IsNumber) {
				resp = this[pos];
				return true;
			}
			return false;
		}
		public bool tryFetch(int pos, ref double resp) {
			if(this[pos] != null && this[pos].IsNumber) {
				resp = this[pos];
				return true;
			}
			return false;
		}

		public bool tryFetch(string pos, ref int resp) {
			if(this[pos] != null && this[pos].IsNumber) {
				resp = this[pos];
				return true;
			}
			return false;
		}
		public bool tryFetch(int pos, ref int resp) {
			if(this[pos] != null && this[pos].IsNumber) {
				resp = this[pos];
				return true;
			}
			return false;
		}

		public bool tryFetch(string pos, ref float resp) {
			if(this[pos] != null && this[pos].IsNumber) {
				resp = this[pos];
				return true;
			}
			return false;
		}
		public bool tryFetch(int pos, ref float resp) {
			if(this[pos] != null && this[pos].IsNumber) {
				resp = this[pos];
				return true;
			}
			return false;
		}

		public bool tryFetch(string pos, ref bool resp) {
			if(this[pos] != null && this[pos].IsBoolean) {
				resp = this[pos];
				return true;
			}
			return false;
		}
		public bool tryFetch(int pos, ref bool resp) {
			if(this[pos] != null && this[pos].IsBoolean) {
				resp = this[pos];
				return true;
			}
			return false;
		}

		public bool tryFetch(string pos, out JSONArray resp) {
			if(this[pos] != null && this[pos].IsArray) {
				resp = this[pos].AsArray;
				return true;
            }
            else {
                resp = null;
                return false;
            }
        }
		public bool tryFetch(int pos, out JSONArray resp) {
			if(this[pos] != null && this[pos].IsArray) {
				resp = this[pos].AsArray;
				return true;
            }
            else {
                resp = null;
                return false;
            }
        }

		public bool tryFetch(string pos, out JSONObject resp) {
			if(this[pos] != null && this[pos].IsObject) {
				resp = this[pos].AsObject;
				return true;
            }
            else {
                resp = null;
                return false;
            }
        }
		public bool tryFetch(int pos, out JSONObject resp) {
            if(this[pos] != null && this[pos].IsObject) {
                resp = this[pos].AsObject;
                return true;
            }
            else {
                resp = null;
                return false;
            }
		}

        public bool tryFetch<T>(string pos, System.Converter<JSONNode, T> converter, out T resp) {
            resp = converter(this[pos]);
            return resp != null;
        }

        public bool tryFetch<T>(int pos, System.Converter<JSONNode, T> converter, out T resp) {
            resp = converter(this[pos]);
            return resp != null;
        }
    }
}