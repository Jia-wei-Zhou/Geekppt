CC = g++ -g -Wall

Test: CodeEvaluation.o Test.o
	$(CC) CodeEvaluation.o Test.o -o CodeEvaluation

Test.o: Test.cpp 
	$(CC) -c Test.cpp 

CodeEvaluation.o: CodeEvaluation.cpp CodeEvaluation.h
	$(CC) -c CodeEvaluation.cpp 

tidy: 
	rm *.o *.txt

clean: 
	rm CodeEvaluation *.o *.txt