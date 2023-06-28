import sys;
import os;

class CommandLineProcessor(object):

    arg_to_handler = { }

    def __init__(self):
        self.arg_to_handler = \
        { \
            "-A": (self.add, 2), \
            "-ADD": (self.add, 2), \
            "-R": (self.remove, 1), \
            "-REMOVE": (self.remove, 1), \
            "-O": (self.optimize, 1), \
            "--OPTIMIZE": (self.optimize, 1), \
            "-B": (self.bundle, 1), \
            "--BUNDLE": (self.bundle, 1), \
            "-H": (self.help, 0), \
            "--HELP": (self.help, 0), \
        };

    def process(self, args):
        length = len(args)
        skip = 0
        for idx in range(0, length):
            if skip > 0:
                skip -= 1;
                continue

            upper_arg = str(args[idx]).upper()
            if upper_arg not in self.arg_to_handler:
                continue

            pair = self.arg_to_handler[upper_arg]
            if pair[1] == 2:
                skip = 2;
                if  idx + 2 >= length:
                    continue
                pair[0](args[idx+1], args[idx+2])
            elif pair[1] == 1:
                skip = 1;
                if  idx + 1 >= length:
                    continue
                pair[0](args[idx+1])
            else:
                skip = 0
                pair[0]()

    def add(self, project, migration_name):
        exit_code = os.system(f"dotnet ef migrations -p {project} add {migration_name}")
        if exit_code != 0:
            raise Exception("add migration failed, see output for details")

    def remove(self, project):
        exit_code = os.system(f"dotnet ef migrations -p {project} remove")
        if exit_code != 0:
            raise Exception("add migration failed, see output for details")


    def optimize(self, arg):
        exit_code = os.system("dotnet ef dbcontext optimize -p " + str(arg))
        if exit_code != 0:
            raise Exception("Optimization failed, see output for details")

    def bundle(self, arg):
        pass

    def help(self):
        print("""Usage:
    ef-tool.py (options)
    -o --optimize  optimize dbcontext (argument required)
    -b --bundle    bundle dbcontext - creates efbundle (argument required)
    -h --help      print usage""")


if __name__ == "__main__":
    processor = CommandLineProcessor()
    processor.process(sys.argv[1:])

