import bdb
import json

#do nothing whenever you encounter exception
class BdbQuit(Exception):
    pass

class TraceDebugger(bdb.Bdb):
    #initialize object of class TraceDebugger
    def __init__(self):
        #inherit parent class 
        super().__init__()
        #intialize a list that will keep track of the lines traced
        self.trace = []
        
    def trace_dispatch(self, frame, event, arg):
        """ Dispatch a trace function for debugged frames based on the event.
        
        This function is installed as the trace function for debugged
        frames. Its return value is the new trace function, which is
        usually itself. The default implementation decides how to
        dispatch a frame, depending on the type of event (passed in as a
        string) that is about to be executed.

        The event can be one of the following:
            line: A new line of code is going to be executed.
            call: A function is about to be called or another code block
                  is entered.
            return: A function or other code block is about to return.
            exception: An exception has occurred.
            c_call: A C function is about to be called.
            c_return: A C function has returned.
            c_exception: A C function has raised an exception.
        
        frame has the attributes:    
        f_back is to the previous stack frame (towards the caller), or None if this is the bottom stack frame;
        f_code is the code object being executed in this frame;
        f_locals is the dictionary used to look up local variables;
        f_globals is used for global variables;
        f_builtins is used for built-in (intrinsic) names;
        f_lineno is the current line number of the frame 
            """        
        
        #A variable to track of the current line number
        line_no = frame.f_lineno-1  #Currently, due to the structure of the input, line_no is offset by 1, so we are subtracting 1 to fix the offset, need to work on it as soon as the input method is speci
        
        """
        frame.f_locals contains a lot of informations we are not concerned with, IGNORED_LOCAL_VARS contains the
        variables we don't need
        """
        IGNORED_LOCAL_VARS = ['__builtins__', '__name__', '__exception__', '__doc__', '__package__', '__loader__', '__spec__', '__annotations__', '__cached__', 'bdb', '__file__', 'BdbQuit', 'TraceDebugger', 'code_to_trace', 'debugger','json']
        
        #initialize a dictionary to store locals
        local_vars = {}
        
        #initialize a dictionary to store globals
        global_vars = {}
        
        #intialize a function to store functions
        functions = {}
                        
        
        """calling frame.f_locals returns a dictionary which we need filter out to only collect the variable values"""
        for key,value in frame.f_locals.items(): #converts the dictionary to a tuple containing the key and the value
            if key not in IGNORED_LOCAL_VARS:    #checks if the key is not in the ignored variable list
                if callable(frame.f_locals[key]) and (event=='call' or event=='return') and key==frame.f_code.co_name:  #checks if the variable is a function, it is either returning a value or being called, and if the current function in the locals is the one that is being executed 
                    if event == "return": #if it is returning value, function dictionary passes the return value
                        functions[key] = {
                            "event": event,
                            "return_value": arg
                            }
                    else:
                        functions[key] = {
                            "event": event, #if it is being called, function dictionary returns nothing
                            "return_value": "none"
                            }
                        
                elif callable(frame.f_locals[key]) and (event!='call' or event!='return'): #checks if the variable is a function and it is not being called or returning a value, i.e the program just read the function name
                    functions[key] = {
                            "event": "null",
                            "return_value": "none"
                            }                    
                else:
                    local_vars[key] = value #other than functions, everything else should be a variable 
        
        """calling frame.f_globals returns a dictionary which we need filter out to only collect the variable values"""
        #repeating the same filtering for global variables
        for key,value in frame.f_globals.items():
            if key not in IGNORED_LOCAL_VARS:
                if callable(frame.f_globals[key]) and (event=='call' or event=='return') and key == frame.f_code.co_name:
                    if event == "return":
                        functions[key] = {
                            "event": event,
                            "return_value": arg
                            }
                    else:
                        functions[key] = {
                            "event": event,
                            "return_value": "none"
                            }
                elif callable(frame.f_globals[key]) and (event!='call' or event!='return'):
                    functions[key] = {
                            "event": "null",
                            "return_value": "none"
                            }              
                else:
                    global_vars[key] = value        
        
        
        #Due to current program, the line number has an offset, the initial line does not contain anything and is read as line 0, due to offset line_number becomes -1 and it is filtered out
        if line_no != -1: #create a dictionary that contains line_number, local variables, global variables, and functions for each executed line        
            trace_dictionary = {
                "line_number": line_no,
                "locals": local_vars,
                "globals": global_vars,
                "functions": functions
            }
            
            """Exception handling: If the event is an exception, we add a new key to our trace dictionary called exception.
            Exception event is tuple containing 3 values, (Exception Type, Exception Message, and Exception Address), we are just concerned with first two
            """
            if event == "exception":
                trace_dictionary["exception"] = {
                    "type": str(arg[0]),   # Exception type
                    "message": str(arg[1]) # Exception message
                }
                self.trace.append(trace_dictionary) #add the exception to the dictionary
                raise BdbQuit #quit the debugger due to exception            
            
            #in case there's no exception raise, add the created dictionary to the list
            self.trace.append(trace_dictionary)
        return self.trace_dispatch #trace the next line
    
    def run_code(self, code):
        """ Run the provided code within the debugger. """
        # Run the code string within the debugging context
        try:
            self.run(code)
        except BdbQuit:
            pass    
        
        
if __name__ == "__main__":  
    # Sample code to trace
    code_to_trace =     """
def calculate():
    a = 10
    b = 3

    
    if a == 2:
        c = a + b
    else:
        c = a + b - subtract()
    return c
    
def subtract():
    a =5
    return a

a = calculate()
    """ 

    # Initialize the debugger
    debugger = TraceDebugger()
    # Run the sample code within the debugger
    debugger.run_code(code_to_trace)
    # store the debugged list in a JSON file 
    with open("debugger.json", mode = "w", encoding="utf-8") as write_file:
        json.dump(debugger.trace, write_file, indent=4)    