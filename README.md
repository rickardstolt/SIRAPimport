# Usage

	SIRAPimport <action> -options

	GlobalOption   Description
	Help (-?)      Shows this help

	Actions

	SportIdentCenter -options - Import punches from SportIdent Center

		Option            Description
		Modem (-M)        List of modem serial numbers (strings), separated by comma
		EventId (-E)      One single event id
		User (-U)         A single user name. (Refers to the modem user, not the modem owner)
		After (-A)        Fetch punches after (>=) this punch time (local time)
		AfterId (-Af)     Get all punches after (>) this id [Default='0']
		Url (-Ur)         Url for retrieving punches [Default='https://center-origin.sportident.com/api/rest/v1/punches']
		SirapHost (-S)    Host for SIRAP receiver [Default='127.0.0.1']
		SirapPort (-Si)   Port for SIRAP receiver [Default='10001']
		Period (-P)       Fetch new punches every period (milliseconds) [Default='5000']
		Timeout (-T)      Connection timeout for connection to SportIdent Center [Default='10000']