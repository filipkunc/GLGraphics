#pragma once

namespace GLWrapper
{
	public ref class DesignModeDevenv
	{
	private:
		static volatile bool initialized = false;
		static bool devenvRunning = false;
	public:
		static property bool DesignMode
		{ 
			bool get()
			{
				if (!initialized)
				{
					devenvRunning = System::Diagnostics::Process::GetCurrentProcess()->ProcessName == "devenv";
					initialized = true;					
				}
				return devenvRunning;
			}
		}
	};
}
